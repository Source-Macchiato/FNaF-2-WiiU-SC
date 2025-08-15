#include <cafe/fs.h>
#include <cafe/os.h>
#include <cstdio>
#include <cstring>
#include <vector>

#define MAX_DIRECTORIES_SIZE 4096
#define MAX_LOG_SIZE 4096
#define MAX_FILE_SIZE (100 * 1024 * 1024) // 100MB max file size
#define MAX_PATH_LENGTH 1024

char directoryList[MAX_DIRECTORIES_SIZE];
char logBuffer[MAX_LOG_SIZE];
std::vector<char> fileDataBuffer;

FSClient* fsClient = NULL;
FSCmdBlock* fsCmdBlock = NULL;
char mountPath[FS_MAX_MOUNTPATH_SIZE];
bool isMounted = false;

void AppendToLogBuffer(const char* message) {
    strncat(logBuffer, message, MAX_LOG_SIZE - strlen(logBuffer) - 1);
}

bool CreateDirectoriesRecursively(const char* fullPath) {
    if (!fullPath || !*fullPath) return false;

    char* tempPath = (char*)MEMAllocFromDefaultHeapEx(MAX_PATH_LENGTH, 0x20);
    if (!tempPath) {
        AppendToLogBuffer("PLUGIN: Failed to allocate tempPath\n");
        return false;
    }

    memset(tempPath, 0, MAX_PATH_LENGTH);
    size_t len = strlen(fullPath);
    size_t tempPos = 0;

    for (size_t i = 0; i < len; ++i) {
        tempPath[tempPos++] = fullPath[i];

        if ((fullPath[i] == '/' && i != 0) || i == len - 1) {
            tempPath[tempPos] = '\0';

            if (strcmp(tempPath, mountPath) == 0 || strcmp(tempPath, "/") == 0) {
                continue;
            }

            FSStat stat;
            FSStatus status = FSGetStat(fsClient, fsCmdBlock, tempPath, &stat, FS_RET_ALL_ERROR);
            if (status == FS_STATUS_NOT_FOUND) {
                status = FSMakeDir(fsClient, fsCmdBlock, tempPath, FS_RET_ALL_ERROR);
                if (status < FS_STATUS_OK) {
                    char errMsg[256];
                    snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSMakeDir failed for %s. Error Code: %d\n", tempPath, status);
                    AppendToLogBuffer(errMsg);
                    MEMFreeToDefaultHeap(tempPath);
                    return false;
                } else {
                    char logMsg[256];
                    snprintf(logMsg, sizeof(logMsg), "PLUGIN: Created directory: %s\n", tempPath);
                    AppendToLogBuffer(logMsg);
                }
            } else if (status < FS_STATUS_OK) {
                char errMsg[256];
                snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSGetStat failed for %s. Error Code: %d\n", tempPath, status);
                AppendToLogBuffer(errMsg);
                MEMFreeToDefaultHeap(tempPath);
                return false;
            }
        }
    }

    MEMFreeToDefaultHeap(tempPath);
    return true;
}

bool MountSDCard() {
    if (isMounted) return true;

    fsClient = (FSClient*)MEMAllocFromDefaultHeap(sizeof(FSClient));
    fsCmdBlock = (FSCmdBlock*)MEMAllocFromDefaultHeap(sizeof(FSCmdBlock));

    if (!fsClient || !fsCmdBlock) {
        AppendToLogBuffer("PLUGIN: Failed to allocate memory for FSClient or FSCmdBlock\n");
        return false;
    }

    FSAddClient(fsClient, FS_RET_NO_ERROR);
    FSInitCmdBlock(fsCmdBlock);

    FSMountSource source;
    FSStatus status = FSGetMountSource(fsClient, fsCmdBlock, FS_SOURCETYPE_EXTERNAL, &source, FS_RET_NO_ERROR);
    if (status < FS_STATUS_OK) {
        char logMessage[256];
        snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSGetMountSource failed. Error Code: %d\n", status);
        AppendToLogBuffer(logMessage);
        return false;
    }

    status = FSMount(fsClient, fsCmdBlock, &source, mountPath, sizeof(mountPath), FS_RET_NO_ERROR);
    if (status < FS_STATUS_OK) {
        char logMessage[256];
        snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSMount failed. Error Code: %d\n", status);
        AppendToLogBuffer(logMessage);
        return false;
    }

    char mountMessage[256];
    snprintf(mountMessage, sizeof(mountMessage), "PLUGIN: Mounted SD card at %s\n", mountPath);
    AppendToLogBuffer(mountMessage);

    isMounted = true;
    return true;
}

void UnmountSDCard() {
    if (!isMounted) return;

    FSUnmount(fsClient, fsCmdBlock, mountPath, FS_RET_NO_ERROR);
    MEMFreeToDefaultHeap(fsClient);
    MEMFreeToDefaultHeap(fsCmdBlock);
    fsClient = NULL;
    fsCmdBlock = NULL;
    isMounted = false;
    
    AppendToLogBuffer("PLUGIN: Unmounted SD card\n");
}

extern "C" {
    int rpl_entry(void* handle, int reason) {
        std::printf("PLUGIN: rpl_entry. handle:%p, reason:%d\n", handle, reason);
        return 0;
    }

    bool ListSDFolders();
    bool ReadFileData(const char* filePath);
    bool WriteFile(const char* filePath, const void* data, int dataSize);
    const char* GetListOfDirectories();
    const char* GetLogMessages();
    const char* GetListOfDirectories();
    const char* GetLogMessages();
    const char* GetFileData();
    int GetFileDataSize();

    bool SDCard_Init() {
        AppendToLogBuffer("PLUGIN: Initializing SD Card Plugin...\n");

        directoryList[0] = '\0';
        logBuffer[0] = '\0';
        fileDataBuffer.clear();
        
        MountSDCard();
        return ListSDFolders();
    }

    void SDCard_Finalize() {
        AppendToLogBuffer("PLUGIN: Finalizing SD Card Plugin...\n");
        UnmountSDCard();
    }

    bool ListSDFolders() {
        if (!isMounted) {
            AppendToLogBuffer("PLUGIN: SD card not mounted in ListSDFolders\n");
            return false;
        }

        AppendToLogBuffer("PLUGIN: Starting ListSDFolders...\n");

        FSStatus status = FSChangeDir(fsClient, fsCmdBlock, mountPath, FS_RET_NO_ERROR);
        if (status < FS_STATUS_OK) {
            char logMessage[256];
            snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSChangeDir failed. Error Code: %d\n", status);
            AppendToLogBuffer(logMessage);
            return false;
        }

        FSDirHandle dirHandle;
        FSDirEntry dirEntry;

        status = FSOpenDir(fsClient, fsCmdBlock, mountPath, &dirHandle, FS_RET_NO_ERROR);
        if (status < FS_STATUS_OK) {
            char logMessage[256];
            snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSOpenDir failed. Error Code: %d\n", status);
            AppendToLogBuffer(logMessage);
            return false;
        }

        AppendToLogBuffer("PLUGIN: Reading directory contents...\n");

        directoryList[0] = '\0';

        while ((status = FSReadDir(fsClient, fsCmdBlock, dirHandle, &dirEntry, FS_RET_NO_ERROR)) == FS_STATUS_OK) {
            strcat(directoryList, dirEntry.name);
            strcat(directoryList, "\n");
        }

        if (status != FS_STATUS_END) {
            char logMessage[256];
            snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSReadDir failed. Error Code: %d\n", status);
            AppendToLogBuffer(logMessage);
        } else {
            AppendToLogBuffer("PLUGIN: End of directory reached\n");
        }

        FSCloseDir(fsClient, fsCmdBlock, dirHandle, FS_RET_NO_ERROR);
        AppendToLogBuffer("PLUGIN: Finished ListSDFolders\n");
        return true;
    }

    bool pl_CheckDirectory(const char* path)
    {
        FSDirHandle dirHandle;
        FSDirEntry dirEntry;

        FSStatus status = FSOpenDir(fsClient, fsCmdBlock, path, &dirHandle, FS_RET_ALL_ERROR);
        if (status < FS_STATUS_OK) {
            char logMessage[256];
            snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSOpenDir failed. Error Code: %d\n", status);
            AppendToLogBuffer(logMessage);
            return false;
        }
        status = FSReadDir(fsClient, fsCmdBlock, dirHandle, &dirEntry, FS_RET_ALL_ERROR);
        if(status < FS_STATUS_OK){
            char logMessage[256];
            snprintf(logMessage, sizeof(logMessage), "PLUGIN: FSReadDir failed, probably doesn't exists. Error Code: %d\n", status);
            AppendToLogBuffer(logMessage);
            return false;
        }

        FSCloseDir(fsClient, fsCmdBlock, dirHandle, FS_RET_ALL_ERROR);
        AppendToLogBuffer("PLUGIN: Finished checking a specific directory\n");
        return true;
    }

    bool FILEExists(const char* filePath)
    {
        if (!isMounted) {
            AppendToLogBuffer("PLUGIN: SD card not mounted in FileExists\n");
            return false;
        }
    
        FSFileHandle fileHandle;
        FSStatus status = FSOpenFile(fsClient, fsCmdBlock, filePath, "r", &fileHandle, FS_RET_ALL_ERROR);
    
        if (status == FS_STATUS_OK) {
            FSCloseFile(fsClient, fsCmdBlock, fileHandle, FS_RET_ALL_ERROR);
            AppendToLogBuffer("PLUGIN: FileExists - File found using FSOpenFile\n");
            return true;
        } else {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: FileExists - FSOpenFile failed. Error Code: %d\n", status);
            AppendToLogBuffer(errMsg);
            return false;
        }
    }


    bool ReadFileData(const char* filePath) {
        if (!isMounted) {
            AppendToLogBuffer("PLUGIN: SD card not mounted in ReadFileData\n");
            return false;
        }
    
        if (strlen(mountPath) + strlen(filePath) + 2 > MAX_PATH_LENGTH) {
            AppendToLogBuffer("PLUGIN: Path too long\n");
            return false;
        }
    
        FSStat stats;
        FSStatus status = FSGetStat(fsClient, fsCmdBlock, filePath, &stats, FS_RET_NO_ERROR);
        if (status < FS_STATUS_OK) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: Could not get stats for file: %s. Error Code: %d\n", filePath, status);
            AppendToLogBuffer(errMsg);
            return false;
        }
    
        if (stats.size <= 0 || stats.size > MAX_FILE_SIZE) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: Invalid file size (%d bytes). Max allowed: %d\n", stats.size, MAX_FILE_SIZE);
            AppendToLogBuffer(errMsg);
            return false;
        }
    
        FSFileHandle fileHandle;
        status = FSOpenFile(fsClient, fsCmdBlock, filePath, "r", &fileHandle, FS_RET_NO_ERROR);
        if (status < FS_STATUS_OK) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSOpenFile failed for %s. Error Code: %d\n", filePath, status);
            AppendToLogBuffer(errMsg);
            return false;
        }
    
        void* alignedBuffer = MEMAllocFromDefaultHeapEx(stats.size, 0x40);
        if (!alignedBuffer) {
            AppendToLogBuffer("PLUGIN: Failed to allocate aligned memory for file read\n");
            FSCloseFile(fsClient, fsCmdBlock, fileHandle, FS_RET_NO_ERROR);
            return false;
        }

        status = FSReadFile(fsClient, fsCmdBlock, alignedBuffer, 1, stats.size, fileHandle, 0, FS_RET_NO_ERROR);
        FSCloseFile(fsClient, fsCmdBlock, fileHandle, FS_RET_NO_ERROR);

        if (status < FS_STATUS_OK) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSReadFile failed. Error Code: %d\n", status);
            AppendToLogBuffer(errMsg);
            MEMFreeToDefaultHeap(alignedBuffer);
            return false;
        }
        
        fileDataBuffer.resize(stats.size);
        memcpy(&fileDataBuffer[0], alignedBuffer, stats.size);

        MEMFreeToDefaultHeap(alignedBuffer);

        char successMsg[256];
        snprintf(successMsg, sizeof(successMsg), "PLUGIN: Successfully read %d bytes from %s\n", stats.size, filePath);
        AppendToLogBuffer(successMsg);
        return true;
    }

    bool WriteFile(const char* filePath, const void* data, int dataSize) {
        if (!isMounted) {
            AppendToLogBuffer("PLUGIN: SD card not mounted in WriteFile\n");
            return false;
        }
    
        if (!data || dataSize <= 0) {
            AppendToLogBuffer("PLUGIN: Invalid data or data size in WriteFile\n");
            return false;
        }
    
        if (strlen(filePath) + 2 > MAX_PATH_LENGTH) {
            AppendToLogBuffer("PLUGIN: Path too long in WriteFile\n");
            return false;
        }
    
        char dirPath[MAX_PATH_LENGTH];
        strncpy(dirPath, filePath, sizeof(dirPath));
        char* lastSlash = strrchr(dirPath, '/');
        if (lastSlash) {
            *lastSlash = '\0';
            if (!CreateDirectoriesRecursively(dirPath)) {
                return false;
            }
        }

    
        FSFileHandle fileHandle;
        FSStatus status = FSOpenFile(fsClient, fsCmdBlock, filePath, "w", &fileHandle, FS_RET_NO_ERROR);
        if (status < FS_STATUS_OK) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSOpenFile failed for %s. Error Code: %d\n", filePath, status);
            AppendToLogBuffer(errMsg);
            return false;
        }
    
        void* alignedBuffer = MEMAllocFromDefaultHeapEx(dataSize, 0x40);
        if (!alignedBuffer) {
            AppendToLogBuffer("PLUGIN: Failed to allocate aligned memory for file write\n");
            FSCloseFile(fsClient, fsCmdBlock, fileHandle, FS_RET_NO_ERROR);
            return false;
        }
        memcpy(alignedBuffer, data, dataSize);
    
        status = FSWriteFile(fsClient, fsCmdBlock, alignedBuffer, 1, dataSize, fileHandle, 0, FS_RET_NO_ERROR);
        FSCloseFile(fsClient, fsCmdBlock, fileHandle, FS_RET_NO_ERROR);
        MEMFreeToDefaultHeap(alignedBuffer);
    
        if (status < FS_STATUS_OK) {
            char errMsg[256];
            snprintf(errMsg, sizeof(errMsg), "PLUGIN: FSWriteFile failed. Error Code: %d\n", status);
            AppendToLogBuffer(errMsg);
            return false;
        }
    
        char successMsg[256];
        snprintf(successMsg, sizeof(successMsg), "PLUGIN: Successfully wrote %d bytes to %s\n", dataSize, filePath);
        AppendToLogBuffer(successMsg);
        return true;
    }

    const char* GetListOfDirectories() {
        return directoryList;
    }

    const char* GetLogMessages() {
        return logBuffer;
    }

    const char* GetFileData() {
        if (fileDataBuffer.empty()) {
            return NULL;
        }
        return &fileDataBuffer[0];
    }

    int GetFileDataSize() {
        return static_cast<int>(fileDataBuffer.size());
    }
}