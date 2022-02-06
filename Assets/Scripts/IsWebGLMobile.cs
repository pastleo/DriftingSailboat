using System.Runtime.InteropServices;

public static class IsWebGLMobile
{
    public static bool Get()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
        #endif

        return false;
    }

   [DllImport("__Internal")]
   public static extern bool IsMobile();
}
