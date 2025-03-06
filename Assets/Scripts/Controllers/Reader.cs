using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Reader
{
    public static async Task<Matrix4x4[]> LoadAddressableText(string Path)
    {
        AsyncOperationHandle Handle = Addressables.LoadAssetAsync<TextAsset>(Path);
        await Handle.Task;

        TextAsset Text = (TextAsset)Handle.Result; 
        string wrappedJson = "{ \"Matrixes\": " + Text.text + " }";
        return JsonUtility.FromJson<MatrixConverter>(wrappedJson).ReturnAsMatrix4x4();
    }
}

public static class Saver
{
    public static async Task SaveText(string fileName, MatrixConverter FileToSave)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        string jsonWrapper = JsonUtility.ToJson(FileToSave);

        int startIndex = jsonWrapper.IndexOf("[");
        int endIndex = jsonWrapper.LastIndexOf("]") + 1;
        string jsonArray = jsonWrapper.Substring(startIndex, endIndex - startIndex);

        await Task.Run(() => File.WriteAllText(fullPath, jsonArray));
    }
}
