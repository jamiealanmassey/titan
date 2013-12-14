﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

/* Other libs */
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Titan
{
    public class Player : Entity
    {
        public uint     mHealth;
        public Sprite[] mHealthBar = new Sprite[11];
        public uint     mSegment;

        public bool       mInvicible;
        private bool      mOnce;
        private Stopwatch mTimer;

        public Player(Vector2f _position, String _file)
        {
            create(_position, _file, 1);
            mHealth  = 100;
            mSegment = 10;

            mInvicible = false;
            mOnce      = false;
            mTimer     = new Stopwatch();

            for (int i = 0; i < 11; i++)
            {
                Texture tex = new Texture("../../resources/health/health" + i + ".png");
                mHealthBar[i] = new Sprite(tex);

                Vector2f pos = new Vector2f();
                pos.X -= 17f;
                pos.Y -= 10f;
                mHealthBar[i].Position = mPosition - pos;
            }
        }

        public override void createBody(World _physics, BodyType _type)
        {
            base.createBody(_physics, _type);
            mBody.FixedRotation = true;
        }

        public override void update(RenderWindow _window)
        {
            input();
            base.update(_window);

            if (ConvertUnits.ToDisplayUnits(mBody.Position.Y) > 800)
                mBody.Position = ConvertUnits.ToSimUnits(300f, 680f);
        }

        public override void render(RenderWindow _window)
        {
            base.render(_window);
            if (!mInvicible)
            {
                mSegment = mHealth / 10;
                for (int i = 0; i < mSegment; i++)
                    _window.Draw(mHealthBar[i]);
            }
            else
            {
                _window.Draw(mHealthBar[10]);
                if (mTimer.ElapsedMilliseconds > 30000f)
                {
                    mTimer.Reset();
                    mInvicible = false;
                }
            }
        }

        public override void addPosition(float _x, float _y)
        {
            base.addPosition(_x, _y);
            for (int i = 0; i < mHealthBar.Length; i++)
            {
                Vector2f pos = new Vector2f();
                pos.X -= 17f;
                pos.Y -= 10f;
                mHealthBar[i].Position = mPosition + pos;
            }
        }

        public override void setPosition(float _x, float _y)
        {
            base.setPosition(_x, _y);
            for (int i = 0; i < mHealthBar.Length; i++)
            {
                Vector2f pos = new Vector2f();
                pos.X -= 17f;
                pos.Y -= 10f;
                mHealthBar[i].Position = mPosition + pos;
            }
        }

        public override void input()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (mPosition.X > 0)
                    mBody.ApplyLinearImpulse(new Vector2(-0.3f * Delta.mDelta, 0f));
                else
                    mBody.ApplyLinearImpulse(new Vector2(0.3f * Delta.mDelta, 0f));
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if(mPosition.Y < 1280)
                    mBody.ApplyLinearImpulse(new Vector2( 0.3f * Delta.mDelta, 0f));
                else
                    mBody.ApplyLinearImpulse(new Vector2(-0.3f * Delta.mDelta, 0f));
            }

            if(Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                if(!mOnce)
                    invincible();
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (mPosition.X > 0 && mPosition.X < 1280)
                    mBody.ApplyLinearImpulse(new Vector2(0f, -0.9f * Delta.mDelta));
            }
        }

        public void invincible()
        {
            mTimer.Start();
            mInvicible = true;
            mOnce = true;
        }
    }
}
