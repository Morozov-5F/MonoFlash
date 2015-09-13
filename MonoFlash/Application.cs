using Microsoft.Xna.Framework;
using System;

using MonoFlash.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoFlash.Events;
using System.Diagnostics;

namespace MonoFlash
{
    class Application : Game
    {
        public static Application application;
        public static Stage stage;
        public static float deltaTime;

        private Sprite rootSprite;

        #if __MOBILE__ || DEBUG
        private SpriteFont debugFont;
        private int frameRate = 0, frameCounter = 0;
        private TimeSpan elapsedTime;
        #endif

        // MonoGame
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		// Mouse input
		private MouseState lastMouseState;
		private MouseState currentMouseState;

        public Application(Sprite root)
        {
            rootSprite = root;

            Application.application = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            string contentDirectory = "Content";
            /*
			#if __IOS__
            contentDirectory += "iOS";
			#elif __OSX__
            contentDirectory += "MacOSX";
			#elif WINDOWS
            contentDirectory = "Content";
			#elif ANDROID 
            contentDirectory = "Content/bin/Android";
			#endif*/
            Content.RootDirectory = contentDirectory;     
            #if __MOBILE__
            graphics.IsFullScreen = true;       
            #endif
            TouchPanel.EnabledGestures = GestureType.HorizontalDrag;
            IsMouseVisible = true;
            Assets.content = Content;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.TargetElapsedTime = new TimeSpan(10000000L / 60L);
			lastMouseState = currentMouseState = Mouse.GetState(); 
        }

        protected override void LoadContent()
        {
            stage = new Stage();
            stage.StageWidth = graphics.GraphicsDevice.Viewport.Width;
            stage.StageHeight = graphics.GraphicsDevice.Viewport.Height;
            stage.stage = stage;

            stage.AddChild(rootSprite);

            #if __MOBILE__ || DEBUG
            //debugFont = Content.Load<SpriteFont>("fonts/menu");
            #endif

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Assets.content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
//            Debug.WriteLine(gameTime.IsRunningSlowly);
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            #if __MOBILE__ || DEBUG
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            #endif

            #if !__IOS__
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            #endif
            base.Update(gameTime);

			// Mouse input
			lastMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

            if (currentMouseState.Position.X >= 0 && currentMouseState.Position.Y >= 0 && 
                currentMouseState.Position.X <= stage.StageWidth && currentMouseState.Position.Y <= stage.StageHeight)
            {
                // Dispatch Event.TOUCH_BEGIN on stage when left button is pressed
                if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    var mouseDownEvent = new Event(Event.TOUCH_BEGIN);
                    mouseDownEvent.position = currentMouseState.Position.ToVector2();
                    stage.DispatchEvent(mouseDownEvent);
                }
                // Dispatch Event.TOUCH_END on stage when left button is released
                if (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                {
                    var mouseUpEvent = new Event(Event.TOUCH_END);
                    mouseUpEvent.position = currentMouseState.Position.ToVector2();
                    stage.DispatchEvent(mouseUpEvent);                
                }
                // Dispatch Event.TOUCH_MOVE on stage when mouse is moved
                if (lastMouseState.Position != currentMouseState.Position && currentMouseState.LeftButton == ButtonState.Pressed 
                    && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    var mouseMoveEvent = new Event(Event.TOUCH_MOVE);
                    mouseMoveEvent.position = currentMouseState.Position.ToVector2();
                    stage.DispatchEvent(mouseMoveEvent);      
                }
            }

			// Touch input
			#if __MOBILE__
			TouchCollection touches = TouchPanel.GetState();
			foreach (TouchLocation touch in touches)
			{
                var pos = touch.Position;
            #if __IOS__
                pos = new Vector2(pos.X * stage.StageWidth / TouchPanel.DisplayWidth, 
                                  pos.Y * stage.StageHeight / TouchPanel.DisplayHeight);
            #endif
				if (touch.State == TouchLocationState.Pressed)
				{
					var touchBeginEvent = new Event(Event.TOUCH_BEGIN);
					touchBeginEvent.position = pos;
					stage.DispatchEvent(touchBeginEvent);
				}
				else if (touch.State == TouchLocationState.Released)
				{
					var touchEndEvent = new Event(Event.TOUCH_END);
					touchEndEvent.position = pos;
					stage.DispatchEvent(touchEndEvent);
				}
				else if (touch.State == TouchLocationState.Moved)
				{
					var touchMoveEvent = new Event(Event.TOUCH_MOVE);
					touchMoveEvent.position = pos;
					stage.DispatchEvent(touchMoveEvent);
				}
			}
			#endif
        }

        protected override void Draw(GameTime gameTime)
        {
            #if __MOBILE__ || DEBUG
            frameCounter++;
            #endif
            GraphicsDevice.Clear(new Color(172, 229, 224));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            stage.Render(spriteBatch, DisplayObject.TRANSFORM_ABSOLUTE);
            #if __MOBILE__ || DEBUG
            Color color = Color.Green;
            if (frameRate >= 55)
            {
                color = Color.Green;
            }
            if (frameRate >= 30 && frameRate < 55)
            {
                color = Color.Yellow;
            }
            if (frameRate < 30)
            {
                color = Color.Red;
            }
            //spriteBatch.DrawString(debugFont, String.Format("FPS: {0}", frameRate), Vector2.Zero, color, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            #endif
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
