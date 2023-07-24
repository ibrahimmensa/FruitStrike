using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using PollfishUnity;

#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

public class PollfishPostProcessor {

    [PostProcessBuildAttribute(100)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {

        if (target != BuildTarget.iOS) {
            UnityEngine.Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
            return;
        }

#if UNITY_EDITOR_OSX
        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

        PBXProject proj = new PBXProject();

        proj.ReadFromString(File.ReadAllText(projPath));

        string targetGuid = proj.GetUnityMainTargetGuid();

        string sr = File.ReadAllText(pathToBuiltProject + "/Podfile");

        if (sr.Contains("use_frameworks! :linkage => :static"))
        {
            string defaultLocationInProj = "Pods/Pollfish";
            const string coreFrameworkName = "Pollfish.xcframework";
            string framework = Path.Combine(defaultLocationInProj, coreFrameworkName);
            string fileGuid = proj.AddFile(framework, framework);
            PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid, fileGuid);
            proj.WriteToFile (projPath);
        } 
        
        if (!proj.GetBuildPropertyForConfig(targetGuid, "LD_RUNPATH_SEARCH_PATHS").Contains("$(inherited)"))
        {
            proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited)");
        }

        if (!proj.GetBuildPropertyForConfig(targetGuid, "LD_RUNPATH_SEARCH_PATHS").Contains("@executable_path/Frameworks"))
        {
            proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
        }

        proj.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
        proj.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        
        proj.WriteToFile(projPath);
#endif      
    }
}
