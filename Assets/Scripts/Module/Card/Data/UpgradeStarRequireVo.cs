namespace game.main
{
    public class UpgradeStarRequireVo
    {
        public string PropName;

        public int PropId;

        public int NeedNum;

        public int CurrentNum;

        public string GetPropTexturePath
        {
            get { return "Prop/" + PropId; }
        }
    }
}