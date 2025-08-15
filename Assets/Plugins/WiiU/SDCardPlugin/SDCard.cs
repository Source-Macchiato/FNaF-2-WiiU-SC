using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.WiiU
{
    public class SDCard : MonoBehaviour
    {
        [DllImport("SDCardPlugin")]
        private static extern bool SDCard_Init();

        [DllImport("SDCardPlugin")]
        private static extern void SDCard_Finalize();

        [DllImport("SDCardPlugin")]
        private static extern IntPtr GetListOfDirectories();

        [DllImport("SDCardPlugin")]
        private static extern IntPtr GetLogMessages();

        [DllImport("SDCardPlugin")]
        private static extern bool ReadFileData(IntPtr filePath);

        [DllImport("SDCardPlugin")]
        private static extern bool WriteFile(IntPtr filePath, byte[] data, int dataSize);

        [DllImport("SDCardPlugin")]
        private static extern IntPtr GetFileData();

        [DllImport("SDCardPlugin")]
        private static extern int GetFileDataSize();

        [DllImport("SDCardPlugin")]
        private static extern bool pl_CheckDirectory(IntPtr path);

        [DllImport("SDCardPlugin")]
        private static extern bool FILEExists(IntPtr path);

        public static void Init()
        {
            if (SDCard_Init())
            {
                ListDirectories();
            }
        }

        public static void DeInit()
        {
            SDCard_Finalize();
        }

        void OnApplicationQuit()
        {
            SDCard_Finalize();
        }

        public static string ListDirectories()
        {
            IntPtr intPtr = GetListOfDirectories();
            string directories = Marshal.PtrToStringAnsi(intPtr);
            return directories;
        }

        public static byte[] ReadAllBytes(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is empty!");
                return new byte[0];
            }
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

            if (ReadFileData(pathConverted))
            {
                IntPtr fileDataPtr = GetFileData();
                int fileSize = GetFileDataSize();
                if (fileSize > 0 && fileDataPtr != IntPtr.Zero)
                {
                    byte[] fileBytes = new byte[fileSize];
                    Marshal.Copy(fileDataPtr, fileBytes, 0, fileSize);

                    Marshal.FreeHGlobal(pathConverted);
                    return fileBytes;
                }
                else
                {
                    Debug.LogError("Failed to read file data or file is empty.");
                    Marshal.FreeHGlobal(pathConverted);
                    return new byte[0];
                }
            }
            else
            {
                Debug.LogError("Failed to read file: " + filePath);
                Marshal.FreeHGlobal(pathConverted);
                return new byte[0];
            }
        }

        public static string ReadAllText(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is empty!");
                return "";
            }
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

            if (ReadFileData(pathConverted))
            {
                IntPtr fileDataPtr = GetFileData();
                int fileSize = GetFileDataSize();
                if (fileSize > 0 && fileDataPtr != IntPtr.Zero)
                {
                    string fileContent = Marshal.PtrToStringAnsi(fileDataPtr, fileSize);
                    Marshal.FreeHGlobal(pathConverted);

                    return fileContent;
                }
                else
                {
                    Debug.LogError("Failed to read file data or file is empty.");
                    Marshal.FreeHGlobal(pathConverted);
                    return "";
                }
            }
            else
            {
                Debug.LogError("Failed to read file: " + filePath);
                Marshal.FreeHGlobal(pathConverted);
                return "";
            }
        }

        public static string[] ReadAllLines(string path)
        {
            string fileContent = ReadAllText(path);
            return fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static void WriteAllText(string filePath, string content)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is empty!");
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError("Content is empty!");
                return;
            }

            byte[] fileBytes = Encoding.UTF8.GetBytes(content);
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

            if (WriteFile(pathConverted, fileBytes, fileBytes.Length))
            {
                Debug.Log("Successfully wrote file: " + filePath);
            }
            else
            {
                Debug.LogError("Failed to write file: " + filePath);
            }

            Marshal.FreeHGlobal(pathConverted);
            return;
        }

        public static void WriteAllBytes(string filePath, byte[] content)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is empty!");
                return;
            }

            if (content.Length <= 0)
            {
                Debug.LogError("Content is empty!");
                return;
            }

            byte[] fileBytes = content;
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(filePath);

            if (WriteFile(pathConverted, fileBytes, fileBytes.Length))
            {
                Debug.Log("Successfully wrote file: " + filePath);
            }
            else
            {
                Debug.LogError("Failed to write file: " + filePath);
            }

            Marshal.FreeHGlobal(pathConverted);
            return;
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            string fileContent = string.Join(Environment.NewLine, contents);

            WriteAllText(path, fileContent);
        }

        public static bool CheckDirectory(string path)
        {
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(path);

            if (!pl_CheckDirectory(pathConverted))
            {
                Marshal.FreeHGlobal(pathConverted);
                return false;
            }

            Marshal.FreeHGlobal(pathConverted);
            return true;
        }

        public static bool FileExists(string path)
        {
            IntPtr pathConverted = Marshal.StringToHGlobalAnsi(path);

            if (!FILEExists(pathConverted))
            {
                Marshal.FreeHGlobal(pathConverted);
                return false;
            }

            Marshal.FreeHGlobal(pathConverted);
            return true;
        }
    }
}

