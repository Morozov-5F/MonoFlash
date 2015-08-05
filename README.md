# MonoFlash
A simple Flash-like API for MonoGame framework

```C#
// Program.cs
...
  static void Main()
  {
      Application application = new Application(new GameMain());
      application.Run();
  }
...
```

```C#
// GameMain.cs
...
class GameMain : Sprite
{
    private Sprite sprite;
    public GameMain()
    {
        addEventListener(Event.ADDED_TO_STAGE, initialize);
    }

    private void initialize(Event e)
    {
        removeEventListener(Event.ADDED_TO_STAGE, initialize);

        BitmapData texture = Assets.getBitmapData("test");
        Bitmap bitmap = new Bitmap(texture);
        // Center bitmap
        bitmap.x = -bitmap.width / 2f;
        bitmap.y = -bitmap.height / 2f;

        sprite = new Sprite();
        addChild(sprite);
        sprite.addChild(bitmap);

        // Center sprite
        sprite.x = stage.stageWidth / 2f;
        sprite.y = stage.stageHeight / 2f;
        // Scale sprite
        sprite.scale = new Vector2(2f);

        addEventListener(Event.ENTER_FRAME, update);
    }

    private void update(Event e)
    {
        sprite.rotation++;
    }
}
```