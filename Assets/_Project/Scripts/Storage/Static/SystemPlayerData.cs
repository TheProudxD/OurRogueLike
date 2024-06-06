﻿using Tools;

namespace Storage.Static
{
    public class SystemPlayerData 
    {
        public static SystemPlayerData Instance;

        public SystemPlayerData(int uID, string key)
        {
            uid = uID;
            this.key = key;
        }

        public readonly int uid;
        public readonly string key;

        public void ToSingleton() => Instance = this;
        public override string ToString() => this.GiveAllFields();

        public override bool Equals(object obj)
        {
            var newData = obj as SystemPlayerData;
            if (newData == null)
                return false;

            return newData.uid == uid;
        }
        
        public override int GetHashCode() => uid.GetHashCode()*19+key.GetHashCode()*13;
    }
}