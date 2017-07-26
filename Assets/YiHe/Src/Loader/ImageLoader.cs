using System;
using System.Collections;
using System.Collections.Generic;
using GDGeek;
using UnityEngine;
namespace YiHe
{
    public class ImageLoader : GDGeek.Singleton<ImageLoader>
    {
        public class LoadTask : Task
        {
            public Texture2D _texture = null;

        };
       
        internal LoadTask load(string url)
        {
            bool isOver = false;
            LoadTask task = new LoadTask();
            task.init = delegate
            {
                Texture2D t2d = loadFromLocal(url);
                if (t2d != null)
                {
                    task._texture = t2d;
                    isOver = true;
                }
                else
                {
                    StartCoroutine(readFromUrl(url, delegate (Texture2D texture)
                    {
                        task._texture = texture;
                        if (texture != null)
                        {
                            saveToLocal(url, texture);

                        }
                        isOver = true;
                    }));
                }
            };

            task.isOver = delegate
            {
                return isOver;
            };
            return task;
        }

      

        private Texture2D loadFromLocal(string imageUrl)
        {

            bool has = PlayerPrefs.HasKey("@image_" + imageUrl) && PlayerPrefs.HasKey("@image_width_" + imageUrl) && PlayerPrefs.HasKey("@image_height_" + imageUrl);
            if (has)
            {
                string str = PlayerPrefs.GetString("@image_" + imageUrl);
                var bytes = Convert.FromBase64String(str);
                Texture2D texture = new Texture2D(PlayerPrefs.GetInt("@image_width_" + imageUrl), PlayerPrefs.GetInt("@image_height_" + imageUrl));
                texture.LoadImage(bytes);

                return texture;
            }

            return null;

        }

        private void saveToLocal(string imageUrl, Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            var str = System.Convert.ToBase64String(bytes);
            PlayerPrefs.SetString("@image_" + imageUrl, str);
            PlayerPrefs.SetInt("@image_width_" + imageUrl, texture.width);
            PlayerPrefs.SetInt("@image_height_" + imageUrl, texture.height);
            PlayerPrefs.Save();
        }


        public IEnumerator readFromUrl(string imageUrl, Action<Texture2D> over)
        {

            // string url = "http://localhost/image/dahuangmao-01.png";
            var www = new WWW(imageUrl);
            yield return new WaitForSeconds(1);
            yield return www;

            if (www.error == null)
            {
                over(www.texture);
               
            }
            else
            {
                over(null);
            }

        }

    }
}