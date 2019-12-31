﻿#region --------------------------文件信息--------------------------------------
/******************************************************************
** 文件名:	AnimSpriteManager
** 版  权:	(C)  
** 创建人:  Unity喵
** 日  期:	
** 描  述: 	
*******************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoveYouForever
{
    public class AnimSpriteManager : InstanceNull<AnimSpriteManager>
    {
        /// <summary>
        /// 图片数据集合
        /// </summary>
        struct PicData
        {
            public int x;
            public int y;
            public int w;
            public int h;
            public int offx;
            public int offy;
        }

        /// <summary>
        /// 数据文件主路径
        /// </summary>
        string dataMainPath;

        /// <summary>
        /// 图片文件主路径
        /// </summary>
        string textureMainPath;

        /// <summary>
        /// 2D资源表
        /// </summary>
        Dictionary<string, Sprite[]> spriteList;

        public AnimSpriteManager()
        {
            spriteList = new Dictionary<string, Sprite[]>();
        }

        /// <summary>
        /// 初始化2D资源相关主路径
        /// </summary>
        /// <param name="mainPath"></param>
        public void Init(string mainPath)
        {
            dataMainPath = mainPath + "/data/";
            textureMainPath = mainPath + "/pic/";
        }

        /* 
            加载动画图集 
            规则：
                bytes放主路径下的data文件夹
                png放主路径下的pic文件夹
            参数：“相对路径#文件名” 用#隔开
        */
        public Sprite[] LoadAnim(string name)
        {
            if (!spriteList.ContainsKey(name))
            {
                // 字符串分割 name.Split('#');
                string[] str = name.Split('#');
                spriteList.Add(name,
                    LoadSprite(textureMainPath + str[0], str[1], dataMainPath + str[0], str[1]));
            }

            return spriteList[name];
        }

        public void RemoveAnim(string name)
        {
            if (!spriteList.ContainsKey(name))
                return;
            spriteList.Remove(name);
        }

        public void ClearAnim()
        {
            spriteList.Clear();
        }

        // TODO: 待修改
        /// <summary>
        /// 精灵资源加载（图片路径，资源名，数据路径，数据名）
        /// </summary>
        /// <param name="texture_path_name"></param>
        /// <param name="texture_res_name"></param>
        /// <param name="data_path_name"></param>
        /// <param name="data_res_name"></param>
        /// <returns></returns>
        Sprite[] LoadSprite(string texture_path_name, string texture_res_name, string data_path_name,
            string data_res_name)
        {
            // 加载图片
            Texture2D texture = ResLoadManager.Instance.LoadObject<Texture2D>(texture_path_name, texture_res_name);
            // 加载数据
            TextAsset data = ResLoadManager.Instance.LoadObject<TextAsset>(data_path_name, data_res_name);
            int index = 0;
            int len = BitConverter.ToInt32(data.bytes, index);
            index += 4;
            // 数据集合
            PicData[] pic_data = new PicData[len];
            // 图片集合
            Sprite[] pic = new Sprite[len];
            // 取数据
            for (int i = 0; i < len; i++)
            {
                pic_data[i].x = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].y = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].w = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].h = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].offx = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].offy = BitConverter.ToInt32(data.bytes, index);
                index += 4;
                pic_data[i].y = texture.height - pic_data[i].y - pic_data[i].h;
            }
 
            // 裁图片
            for (int i = 0; i < len; i++)
            {
                pic[i] = Sprite.Create(texture, new Rect(pic_data[i].x, pic_data[i].y, pic_data[i].w, pic_data[i].h),
                    new Vector2(pic_data[i].offx / pic_data[i].w, pic_data[i].offy / pic_data[i].h));
            }

            // 返回裁剪好的精灵图组
            return pic;
        }
    }
}