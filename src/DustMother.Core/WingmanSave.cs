using UnSave;

namespace DustMother.Core
{
    public abstract class WingmanSave
    {
        protected WingmanSave(GvasSaveData rawSaveData)
        {
            RawSaveData = rawSaveData;
        }

        protected GvasSaveData RawSaveData {get;}
    }
}