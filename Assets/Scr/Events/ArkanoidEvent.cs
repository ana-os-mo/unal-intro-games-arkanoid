public static class ArkanoidEvent 
{
   public delegate void BallDeadZoneAction(Ball ball);
   public static BallDeadZoneAction OnBallReachDeadZoneEvent;

   public delegate void BlockDestroyedAction(int blockID);
   public static BlockDestroyedAction OnBlockDestroyedEvent;
}
