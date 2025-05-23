namespace Assets.Scripts.Sprite
{
    public static class PlayerYPositionBroadCaster
    {
        public static float PlayerYPosition {  get; private set; }

        public static void SetPublicPlayerSortingOrder(IYPositionBroadcastable setter, float input)
        {
            if (setter == null)
            {
                return;
            }
            PlayerYPosition = input;
        }
    }
}
