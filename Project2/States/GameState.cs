using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spextria.Graphics;
using Spextria.Entities;
using Spextria.Maps;
using Spextria.Master;
using Microsoft.Xna.Framework.Input;
using Spextria.Statics;
using MonoGame.Extended.Tiled.Renderers;

namespace Spextria.States
{
    class GameState : State
    {
        public override void Update(MasterManager master, GameTime gameTime, SpextriaGame game, GraphicsDevice graphicsDevice)
        {
            master.entityManager.Player.Update(master);
            master.mapManager.CurrentMap.UpdateObjects(master);
            master.entityManager.UpdateSprites(master);
            master.playGuiManager.Update(master);
            int offsetX = 16;
            int offsetY = 32;

            //_cameraManager.MoveCamera(gameTime, _spriteManager.Player.GetPosition());

            Vector2 scaledPos = master.entityManager.Player.GetPosition();
            Vector2 newCameraPos = new Vector2(scaledPos.X - 440 / 2 + offsetX, scaledPos.Y - 268 / 2 + offsetY);
            Vector2 oldCameraPos = master.cameraManager.Camera.Position;
            Vector2 finalCameraPos = new Vector2( oldCameraPos.X + (newCameraPos.X - oldCameraPos.X) * 0.15f, oldCameraPos.Y + (newCameraPos.Y - oldCameraPos.Y) * 0.12f);
            if (!master.mapManager.CurrentMap.LevelCompleted())
                master.cameraManager.Camera.Position = finalCameraPos;
            //master.cameraManager.Camera.LookAt(new Vector2(finalCameraPos.X, finalCameraPos.Y));
        }
        public override void Draw(MasterManager master, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

            var matrix = master.cameraManager.Camera.GetViewMatrix();


            if (!master.mapManager.CurrentMapDrawn)
            {
                graphicsDevice.SetRenderTarget(master.MapTarget);
                spriteBatch.Begin(effect: master.CurrentSpriteEffect);
                Texture2D mapImage = master.mapManager.CurrentMap.GetMap();
                graphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(mapImage, new Rectangle(0,0,mapImage.Width, mapImage.Height), Color.White);
                spriteBatch.End();
                master.mapManager.CurrentMapDrawn = true;
            }
            graphicsDevice.SetRenderTarget(master.EntitiesTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(transformMatrix: matrix, samplerState: SamplerState.PointClamp, effect: master.CurrentSpriteEffect);
            //_spriteBatch.Draw(_text.BoxImage, _text.Position, Color.White);
            
            master.mapManager.CurrentMap.DrawObjects(spriteBatch);
            master.entityManager.DrawSprites(spriteBatch, master.cameraManager, master, graphicsDevice);
            master.entityManager.Player.Draw(spriteBatch);
            //spriteBatch.End();
            //graphicsDevice.SetRenderTarget(normScreen);
            //spriteBatch.Begin(transformMatrix: staticMatrix);
            master.playGuiManager.Draw(spriteBatch, master);
            Vector2 drawPos = master.cameraManager.GetFixedScreenPos(350, 10);
            drawPos = new Vector2(350 + master.cameraManager.Camera.Position.X / 9, 10 + master.cameraManager.Camera.Position.Y / 9);
            //DebugRect(_spriteBatch, _spriteManager.CollectablesArray[0].GetHitbox().Body, Color.Red);
            //_spriteBatc
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(master.MapTarget, new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height), new Rectangle((int)Math.Round(master.cameraManager.Camera.Position.X), (int)Math.Round(master.cameraManager.Camera.Position.Y), 426, 233), Color.White * master.MapOpacity);
            
            spriteBatch.Draw(master.EntitiesTarget, new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height), Color.White);
            //spriteBatch.Draw(normScreen, new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height), Color.White);
            spriteBatch.End();
        }
    }
}
