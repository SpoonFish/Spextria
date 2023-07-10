using System;
using System.Collections.Generic;
using System.Text;
using Spextria.Graphics;
using Spextria.Entities;
using Spextria.Maps;
using Spextria.StoredData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Spextria.Statics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework.Input;

namespace Spextria.Master
{
    class MasterManager
    {
        public Effect CurrentSpriteEffect;
        public TiledMapEffect CurrentMapEffect;
        public MouseState CurrentMouseState;
        public Point MousePos;
        public Vector2 GraphicsPositionScale;
        public double dt;
        public bool levelFailed;
        public bool NpcTalking;
        public double NpcTalkTime;
        public double timePassed;
        public float GameSpeed;
        public float MapOpacity;
        public GameTime gameTime;
        public MapManager mapManager;
        public EntityManager entityManager;
        public CameraManager cameraManager;
        public RenderTarget2D MapTarget;
        public RenderTarget2D MenuTarget;
        public RenderTarget2D EntitiesTarget;

        public Point savedScroll;
        public bool levelWon;

        public StoredDataManager storedDataManager;
        public PlayScreenGuiManager playGuiManager;

        public MasterManager(GameWindow window, GraphicsDevice graphics, GraphicsDeviceManager graphicsDeviceManager, ContentManager content)
        {
            float XScale = graphics.DisplayMode.Width / 1920f;
            float YScale = (graphics.DisplayMode.Height+30) / 1080f;
            GraphicsPositionScale = new Vector2(XScale, YScale);

            if (graphics.DisplayMode.Width == 1920 && graphics.DisplayMode.Height == 1080)
                GraphicsPositionScale = new Vector2(1,1);

            MapOpacity = 1;
            NpcTalking = false;
            NpcTalkTime = 0;
            MousePos = new Point(0, 0);
            GameSpeed = 1f;
            MenuTarget = new RenderTarget2D(graphics, 426, 233);
            EntitiesTarget = new RenderTarget2D(graphics, 426, 233);
            savedScroll = new Point();
            levelWon = false;
            levelFailed = false;
            CurrentSpriteEffect = null;
            CurrentMapEffect = null;
            storedDataManager = new StoredDataManager();

            cameraManager = new CameraManager(window, graphics, graphicsDeviceManager);
            cameraManager.Camera.Position = new Vector2(-1300, -285);

            PlayerEntity player = new PlayerEntity(Images.ImageDict["player1"], new Vector2(40, 18), new PlayerStats(),2f, 2.7f, weight: 1.8f);
            entityManager = new EntityManager(player);

            mapManager = new MapManager(content, graphics, entityManager);
            MapTarget = new RenderTarget2D(graphics, 426, 233);

            playGuiManager = new PlayScreenGuiManager(entityManager);
        }
           
    }
}
