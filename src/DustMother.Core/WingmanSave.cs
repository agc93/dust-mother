using UnSave;

namespace DustMother.Core
{
    public abstract class WingmanSave
    {
        protected WingmanSave(GvasSaveData rawSaveData)
        {
            RawSaveData = rawSaveData;
        }

        internal virtual protected GvasSaveData RawSaveData {get;}

        public abstract string FileName {get;}

        public static implicit operator GvasSaveData(WingmanSave save) => save.RawSaveData;
    }
}