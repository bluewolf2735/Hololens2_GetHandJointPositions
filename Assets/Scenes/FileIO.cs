using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;

#if !UNITY_EDITOR && UNITY_METRO
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class FileIO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteString(string s)
    {
        string folderPath;
#if !UNITY_EDITOR && UNITY_METRO
        folderPath = ApplicationData.Current.RoamingFolder.Path;
#else
        folderPath = Application.persistentDataPath;
#endif
        using (Stream stream = OpenFileForWrite(folderPath, "test.txt"))
        {
            byte[] data = Encoding.ASCII.GetBytes(s);
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        Debug.Log(Path.Combine(Application.persistentDataPath, "test.txt"));
    }


    public void WriteTestString()
    {
        string folderPath;
#if !UNITY_EDITOR && UNITY_METRO
        folderPath = ApplicationData.Current.RoamingFolder.Path;
#else
        folderPath = Application.persistentDataPath;
#endif
        using (Stream stream = OpenFileForWrite(folderPath, "test.txt"))
        {
            byte[] data = Encoding.ASCII.GetBytes("2345678TESTEST");
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        Debug.Log(Path.Combine(Application.persistentDataPath, "test.txt"));
    }

    private static Stream OpenFileForWrite(string folderName, string fileName)
    {
        Stream stream = null;
#if !UNITY_EDITOR && UNITY_METRO
        Task<Task> task = Task<Task>.Factory.StartNew(
            async () =>
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                stream = await file.OpenStreamForWriteAsync();
            });
        task.Start();
        task.Result.Wait();
#else
        stream = new FileStream(Path.Combine(folderName, fileName), FileMode.Append, FileAccess.Write);
#endif
        return stream;
    }


    public void WriteTestStringFile()
    {
        string HololensTime = DateTime.Now.ToString();
        string folderPath;

#if !UNITY_EDITOR && UNITY_METRO
        
        folderPath = ApplicationData.Current.RoamingFolder.Path;

        Task<Task> task = Task<Task>.Factory.StartNew(
            async () =>
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
                StorageFile file = await folder.GetFileAsync("test.txt");
                //StorageFile file = await folder.CreateFileAsync("test.txt", CreationCollisionOption.ReplaceExisting);
                Windows.Storage.FileIO.AppendTextAsync(file, HololensTime + "\n");
        
                //plan B , backup 
                //using(var txtFile = new FileStream(Path.Combine(folderPath, "test.txt"), FileMode.Append)){
                //    using(var writer = new StreamWriter(txtFile, Encoding.UTF8)){
                //        writer.Write(HololensTime + "\n");
                //    }
                //}
            });
#else
        folderPath = Application.persistentDataPath;

        using (Stream stream = OpenFileForWrite(folderPath, "test.txt"))
        {
            byte[] data = Encoding.ASCII.GetBytes(HololensTime + "\n");
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        Debug.Log(Path.Combine(Application.persistentDataPath, "test.txt"));

#endif
    }


    public void WriteStringToFile(string s, string fileName, string fileExtension)
    {
        string folderPath;

#if !UNITY_EDITOR && UNITY_METRO
        
        folderPath = ApplicationData.Current.RoamingFolder.Path;

        Task<Task> task = Task<Task>.Factory.StartNew(
            async () =>
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
                StorageFile file = await folder.GetFileAsync(String.Format("{0}.{1}", fileName, fileExtension));
                Windows.Storage.FileIO.AppendTextAsync(file, s + "\n");
            });
#else
        folderPath = Application.persistentDataPath;

        using (Stream stream = OpenFileForWrite(folderPath, String.Format("{0}.{1}", fileName, fileExtension)))
        {
            byte[] data = Encoding.ASCII.GetBytes(s + "\n");
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        Debug.Log(Path.Combine(Application.persistentDataPath, String.Format("{0}.{1}", fileName, fileExtension)));

#endif
    }

    public void CreateDataFile(string fileName, string fileExtension)
    {
        string folderPath;

#if !UNITY_EDITOR && UNITY_METRO
        
        folderPath = ApplicationData.Current.RoamingFolder.Path;

        Task<Task> task = Task<Task>.Factory.StartNew(
            async () =>
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
                StorageFile file = await folder.CreateFileAsync(String.Format("{0}.{1}", fileName, fileExtension), CreationCollisionOption.ReplaceExisting);
            });
#else
        folderPath = Application.persistentDataPath;

        Stream stream = new FileStream(Path.Combine(folderPath, fileName), FileMode.CreateNew, FileAccess.Write);
        stream.Flush();
        Debug.Log(Path.Combine(Application.persistentDataPath, String.Format("{0}.{1}", fileName, fileExtension)));

#endif
    }

}
