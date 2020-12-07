public class JenkinsBuild
{
    /// <summary>
    /// 此方法是从jienkins上接受数据的方法
    /// </summary>
    static void CommandLineBuild()
    {
        try
        {
            Debug.Log("Command line build\n------------------\n------------------");
            string[] scenes = GetBuildScenes();
            //这里的路径是打包的路径,自定义
            string path = "E:/Publish/cloudRes/version/Temp"; 
            Debug.Log(path);
            for (int i = 0; i < scenes.Length; ++i)
            {    
                Debug.Log($"Scene[{i}]: \"{scenes[i]}\"");
            }
            Debug.Log("Starting Build!");
            Debug.Log(GetJenkinsParameter("Platform"));
            Build(path);
        }
        catch (Exception err)
        {
            Console.WriteLine("方法F中捕捉到：" + err.Message);
            throw; //重新抛出当前正在由catch块处理的异常err
        }
        finally
        {
            Debug.Log("---------->  I am copying!   <--------------");
        }
    }
    /// <summary>
    ///解释jekins 传输的参数
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static string GetJenkinsParameter( string name )
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith(name))
            {
                return arg.Split("-"[0])[1];
            }
        }
        return null;
    }
    
    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="targetPath"></param>
    static void Build(string targetPath )
    {
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
        string version = GetJenkinsParameter("Version");
        string resVersion = GetJenkinsParameter("Version");
        string buildTarget = GetJenkinsParameter("BuildTarget");
        
        string curPath = $"{targetPath}/{PlayerSettings.productName}_{version}_{DateTime.Now:yyyy_MM_dd_HH_mm}.apk";
    
        if (Enum.TryParse(buildTarget,true,out BuildTarget target))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target),target);
        }
        BuildPipeline.BuildPlayer(GetBuildScenes(), curPath, target, BuildOptions.None);
        Debug.Log("Build Success");
    }
    
    /// <summary>
    /// 获取编辑器构建场景列表
    /// </summary>
    /// <returns></returns>
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;

            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }
}
