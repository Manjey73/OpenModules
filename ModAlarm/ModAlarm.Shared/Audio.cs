using System.Runtime.InteropServices;

namespace Scada.Server.Modules.ModAlarm
{
    public static class Wav
    {
        [DllImport("winmm.dll", SetLastError = true)]
        private static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound); // string pszSound, UIntPtr hmod, uint fdwSound

        [Flags]
        public enum SoundFlags
        {
            SND_SYNC = 0x0000,/* play synchronously (default) */
            SND_ASYNC = 0x0001, /* play asynchronously */
            SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004, /* pszSound points to a memory file */
            SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000,/* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a pre d ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004, /* name is resource name or atom */
            SND_PURGE = 0x0040,  /* purge non-static events for task */
            SND_APPLICATION = 0x0080, /* look for application specific association */
            SND_SENTRY = 0x00080000, /* Generate a SoundSentry event with this sound */
            SND_RING = 0x00100000, /* Treat this as a "ring" from a communications app - don't duck me */
            SND_SYSTEM = 0x00200000 /* Treat this as a system sound */
        }

        public static void Play(string strFileName, SoundFlags flags)
        {
            PlaySound(strFileName, UIntPtr.Zero, (uint)flags);
        }
    }
}
