/*
    FmodGuids.cs - FMOD Studio API

    Generated GUIDs for project 'Jam5_Arcade.fspro'
*/

using System;
using System.Collections.Generic;

namespace Audio
{
    public class AudioEvent
    {
        public static readonly FMOD.GUID DestroyEnemy = new FMOD.GUID { Data1 = -1472737554, Data2 = 1307535774, Data3 = -785032035, Data4 = -688278640 };
        public static readonly FMOD.GUID PlayerMovement = new FMOD.GUID { Data1 = -9758484, Data2 = 1245021375, Data3 = 1395276450, Data4 = 767652684 };


        public static readonly Dictionary<string, FMOD.GUID> AudioEventNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"DestroyEnemy", DestroyEnemy}, {"PlayerMovement", PlayerMovement}, 
        };
    }

    public class AudioBus
    {
        public static readonly FMOD.GUID MasterBus = new FMOD.GUID { Data1 = -988115165, Data2 = 1228846850, Data3 = -1090998861, Data4 = 252002649 };
        public static readonly FMOD.GUID Reverb = new FMOD.GUID { Data1 = -1710510160, Data2 = 1140030815, Data3 = -632194644, Data4 = 2090800667 };


        public static readonly Dictionary<string, FMOD.GUID> AudioBusNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"MasterBus", MasterBus}, {"Reverb", Reverb}, 
        };
    }

    public class AudioBank
    {
        public static readonly FMOD.GUID Master = new FMOD.GUID { Data1 = -1450428026, Data2 = 1150626526, Data3 = -370190429, Data4 = -1375635118 };


        public static readonly Dictionary<string, FMOD.GUID> AudioBankNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"Master", Master}, 
        };
    }

}

