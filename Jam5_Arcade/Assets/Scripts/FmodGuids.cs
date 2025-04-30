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
        public static readonly FMOD.GUID GameOver = new FMOD.GUID { Data1 = 1415682934, Data2 = 1333054095, Data3 = -2088017772, Data4 = 1718655380 };
        public static readonly FMOD.GUID PlayerMovement = new FMOD.GUID { Data1 = 630502631, Data2 = 1200581693, Data3 = 654225086, Data4 = -185438848 };
        public static readonly FMOD.GUID SpawnEnemy = new FMOD.GUID { Data1 = -539728580, Data2 = 1137822222, Data3 = -371316585, Data4 = 47771630 };


        public static readonly Dictionary<string, FMOD.GUID> AudioEventNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"DestroyEnemy", DestroyEnemy}, {"GameOver", GameOver}, {"PlayerMovement", PlayerMovement}, {"SpawnEnemy", SpawnEnemy}, 
        };
    }

    public class AudioBus
    {
        public static readonly FMOD.GUID MasterBus = new FMOD.GUID { Data1 = -988115165, Data2 = 1228846850, Data3 = -1090998861, Data4 = 252002649 };
        public static readonly FMOD.GUID Enemies = new FMOD.GUID { Data1 = -2019629448, Data2 = 1214501383, Data3 = -2144331354, Data4 = 794609143 };
        public static readonly FMOD.GUID Reverb = new FMOD.GUID { Data1 = -1710510160, Data2 = 1140030815, Data3 = -632194644, Data4 = 2090800667 };


        public static readonly Dictionary<string, FMOD.GUID> AudioBusNameToGuid = new Dictionary<string, FMOD.GUID>()
        {
                {"MasterBus", MasterBus}, {"Enemies", Enemies}, {"Reverb", Reverb}, 
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

