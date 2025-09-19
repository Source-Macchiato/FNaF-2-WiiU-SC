using System.Text;
using System.IO;
using System.Threading;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class SaveGameState : MonoBehaviour
{
    public static int saveResult = -1;
    public static bool isSaving = false;

    public static void DoSave(byte[] data)
    {
        string path = Application.persistentDataPath + "/data.sav";
        saveResult = -1;
        isSaving = true;
        Thread t = new Thread(new ThreadStart(
            delegate
            {
                DelegatedSave(data, path);
            })
        );

        t.Start();
    }

    private static void DelegatedSave(byte[] data, string path)
    {
        WiiU.SaveCommand cmd = WiiU.Save.SaveCommand(WiiU.Save.accountNo);

        long freespace = 0;
        WiiU.Save.FSStatus status = cmd.GetFreeSpaceSize(out freespace, WiiU.Save.FSRetFlag.None);
        if (status != WiiU.Save.FSStatus.OK)
        {
            saveResult = 0;
            isSaving = false;
        }

        long needspace = Mathf.Max(1024 * 1024, data.Length);

        if (freespace < needspace)
        {
            // not enough free space
            saveResult = 0;
            isSaving = false;
        }
        else
        {
            var fileStream = new FileStream(path, FileMode.Create);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();

            // It is very important to flush quota, otherwise filesystem changes will be discarded upon reboot
            status = cmd.FlushQuota(WiiU.Save.FSRetFlag.None);
            if (status != WiiU.Save.FSStatus.OK)
            {
                saveResult = 0;
            }
        }

        saveResult = 1;
        isSaving = false;
    }

    public static string DoLoad()
    {
        try
        {
            using (var fileStream = new FileStream(Application.persistentDataPath + "/data.sav", FileMode.Open))
            {
                var dataSize = (int)fileStream.Length;

                if (dataSize <= 0)
                {
                    return string.Empty;
                }

                byte[] data = new byte[dataSize];

                if (fileStream.Read(data, 0, dataSize) < dataSize)
                {
                    return string.Empty;
                }

                string json = Encoding.UTF8.GetString(data);

                fileStream.Close();

                return json;
            }
        }
        catch (FileNotFoundException)
        {
            return string.Empty;
        }
        catch (IOException)
        {
            return string.Empty;
        }
    }
}