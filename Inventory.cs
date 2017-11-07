using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DreamCatcher 
{
    public class Inventory
    {
        #region Variables
        //Инвентарь вообще
        List<Object> objectList;

        const int capacity = 10;

        Texture2D invetoryFrame;
        Texture2D pointers;
        Rectangle leftPointerCollRect;
        Rectangle rightPointerCollRect;

        //"Видимая" часть инвентаря
        Object[] visible = new Object[5];
        #endregion

        #region Constructors
        public Inventory()
        {
            objectList = new List<Object>(capacity);
            //invetoryFrame = MainClass.Load<Texture2D>(@"Images\InventoryFrame");
            //pointers = MainClass.Load<Texture2D>(@"Images\InventoryPointers");
        }

        public Inventory(int capacity)
        {
            objectList = new List<Object>(capacity);
            //invetoryFrame = MainClass.Load<Texture2D>(@"Images\InventoryFrame");
            //pointers = MainClass.Load<Texture2D>(@"Images\InventoryPointers");
        }
        #endregion

        #region Methods
        public void IncreaseCapacity(int increase)
        {
            if (increase > 0)
            {
                objectList.Capacity += objectList.Capacity + increase;
            }
        }

        public void Add(Object item)
        {
            if (objectList.Count < objectList.Capacity) objectList.Add(item);

        }

        public void Throw(Object item)
        {
            if (objectList.Contains(item)) objectList.Remove(item);
        }

        public string GetInfo(int index)
        {
            return objectList[index].GetInfo;
        }

        public string GetInfo(string ID)
        {
            foreach(Object o in objectList)
            {
                if (o.GetID == ID) return o.GetInfo;
            }
            return "*nothing*";
        }

        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            if(state.LeftButton == ButtonState.Pressed)
            {
                //Логика "стрелочек" для просмотра инвентаря
                if(new Rectangle(state.Position, new Point(10, 10)).Intersects(leftPointerCollRect))
                {
                    for(int i = 0; i<5; i++)
                    {
                        if (objectList.IndexOf(visible[i]) != 0)
                            visible[i] = objectList[objectList.IndexOf(visible[i]) - 1];
                        else break;
                    }
                }
                else if (new Rectangle(state.Position, new Point(10, 10)).Intersects(rightPointerCollRect))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (objectList.IndexOf(visible[4]) != objectList.Capacity-1)
                            visible[i] = objectList[objectList.IndexOf(visible[i]) + 1];
                        else break;
                    }
                }

                //Логика для "иконок"
                if(state.Position.X > 0 && state.Position.X<800)
                {
                    if(state.Position.Y > 0 && state.Position.Y < 600)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //На случай если spriteBatch уже активен
            //try
            //{
            //    spriteBatch.Begin();
            //    foreach (Object o in visible)
            //    {
            //        o.DrawIcon(gameTime, spriteBatch, new Vector2(0, 0));
            //    }
            //    spriteBatch.Draw(invetoryFrame, new Rectangle(0, 0, 0, 0), Color.White);
            //    spriteBatch.Draw(pointers, new Rectangle(0, 0, 0, 0), Color.White);
            //}
            //catch
            //{
            //    foreach (Object o in visible)
            //    {
            //        o.DrawIcon(gameTime, spriteBatch, new Vector2(0, 0));
            //    }
            //    spriteBatch.Draw(invetoryFrame, new Rectangle(0, 0, 0, 0), Color.White);
            //    spriteBatch.Draw(pointers, new Rectangle(0, 0, 0, 0), Color.White);
            //}
        }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return objectList.Count;
            }
        }

        public List<Object> GetObjects
        {
            get
            {
                return objectList;
            }
        }

        public int Capacity
        {
            get
            {
                return objectList.Capacity;
            }
            set
            {
                if(value > objectList.Capacity)
                {
                    objectList.Capacity = value;
                }
            }
        }
        #endregion

        #region Indexers
        public Object this[int index]
        {
            get
            {
                return objectList[index];
            }
        }
        #endregion
    }
}
