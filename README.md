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
        AddEventListener(Event.ADDED_TO_STAGE, Initialize);
    }

    private void Initialize(Event e)
    {
        RemoveEventListener(Event.ADDED_TO_STAGE, Initialize);

        BitmapData texture = Assets.GetBitmapData("test");
        Bitmap bitmap = new Bitmap(texture);
        // Center bitmap
        bitmap.X = -bitmap.width / 2f;
        bitmap.Y = -bitmap.height / 2f;

        sprite = new Sprite();
        AddChild(sprite);
        sprite.AddChild(bitmap);

        sprite.X = stage.stageWidth / 2f;
        sprite.Y = stage.stageHeight / 2f;

        AddEventListener(Event.ENTER_FRAME, Update);
    }

    private void Update(Event e)
    {
        sprite.rotation++;
    }
}
```
