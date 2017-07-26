using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek
{
    namespace Net
    { 
        /// <summary>
        /// 共享管理，采用了自动列表模式来管理所有的共享（pointer）。
        /// </summary>
        /// <seealso cref="GDGeek.Singleton{HoloGeek.ShareManager}" />
        public class FunctorManager : GDGeek.Singleton<FunctorManager>
        {

    
       
            internal void functor(Functor.Handler handler)
            {
                map_[handler.shareId].callback(handler.func, handler.parameter);
            }
       
            public IFunctor getFunctor(string shareId)
            {
                if (map_.ContainsKey(shareId))
                {
                    return map_[shareId];
                }
                return null;
            }


            private Dictionary<string, IFunctor> map_ = new Dictionary<string, IFunctor>();
            public void add(IFunctor pointer)
            {
                map_.Add(pointer.shareId, pointer);
            }
            public void remove(IFunctor pointer)
            {
                map_.Remove(pointer.shareId);
            }
   
        }
    }
}