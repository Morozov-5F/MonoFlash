using Microsoft.Xna.Framework;
using System;

using MonoFlash.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoFlash
{
    class Application : Game
    {
        public static Application application;
        public static Stage stage;

        private Sprite rootSprite;

        // MonoGame
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Application(Sprite root)
        {
            rootSprite = root;

            Application.application = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Assets.content = Content;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            stage = new Stage();
            stage.stageWidth = graphics.GraphicsDevice.Viewport.Width;
            stage.stageHeight = graphics.GraphicsDevice.Viewport.Height;
            stage.stage = stage;
            stage.AddChild(rootSprite);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Assets.content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            stage.Render(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
