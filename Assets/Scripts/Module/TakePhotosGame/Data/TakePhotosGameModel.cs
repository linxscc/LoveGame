using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using System;
using game.main;

namespace DataModel
{
    public class TakePhotosGameModel : Model
    { 
        List<TakePhotoRulePB> _takePhotoRulePBs;
        List<TakePhotoBuyCountRulePB> _takePhotoBuyCountRulePBs;
        
        UserTakePhotoInfoPB _userTakePhotoInfoPB;

        TakePhotosGameRunningInfo _takePhotosGameRunningInfo;
        int _takeCount = 0;//已拍照次数
        int _buyCount = 0;//购买拍照次数

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="res"></param>
        public void InitRunningInfo(StartTakePhotoRes res)
        {
            _takePhotosGameRunningInfo = new TakePhotosGameRunningInfo();
            /////test
            //for(int i=18;i< _takePhotoRulePBs.Count;i++)
            //{
            //    var v = _takePhotoRulePBs[i];
            //    var rule = GetRulePbById(v.Id);
            //    if (rule == null)
            //    {
            //        Debug.LogError("don't find rule id" + rule.Id);
            //        continue;
            //    }
            //    var vo = new TakePhotoGamePhotoVo(rule);
            //    vo.Id = v.Id;
            //    _takePhotosGameRunningInfo.AddTakePhotoGamePhotoVo(vo);
            //}
            ////test end

            foreach (var v in res.PictureId)
            {
                var rule = GetRulePbById(v);
                if (rule == null)
                {
                    Debug.LogError("don't find rule id" + rule.Id);
                    continue;
                }
                var vo = new TakePhotoGamePhotoVo(rule);
                vo.Id = v;
                _takePhotosGameRunningInfo.AddTakePhotoGamePhotoVo(vo);
            }
            _takePhotosGameRunningInfo.Init();
        }

        /// <summary>
        /// 获取运行数据
        /// </summary>
        /// <returns></returns>
        public TakePhotosGameRunningInfo GetRunningInfo()
        {
            return _takePhotosGameRunningInfo;
        }

        /// <summary>
        /// 初始化规则
        /// </summary>
        /// <param name="resU"></param>
        public void InitRule(TakePhotoRules resU)
        {
            _takePhotoRulePBs = new List<TakePhotoRulePB>();
            _takePhotoRulePBs.AddRange(resU.TakeTuleRules);

            _takePhotoBuyCountRulePBs = new List<TakePhotoBuyCountRulePB>();
            _takePhotoBuyCountRulePBs.AddRange(resU.TakePhotoBuyCountRules);
        }






        /// <summary>
        /// 更新购买次数
        /// </summary>
        /// <param name="userTakePhotoInfo"></param>
        public void UpdateBuyCount(UserTakePhotoInfoPB userTakePhotoInfo)
        {
            _takeCount = userTakePhotoInfo.TakeCount;
            _buyCount = userTakePhotoInfo.BuyCount;
        }

        /// <summary>
        /// 初始化购买次数
        /// </summary>
        /// <param name="resV"></param>
        public void InitCount(GetUserTakePhotoInfoRes resV)
        {
 
            _takeCount = resV.TakeCount;
            _buyCount = resV.BuyCount;
        }


        public int GetConsumeTime()
        {
            return _takeCount;
        }


        /// <summary>
        /// 获取上限
        /// </summary>
        /// <returns></returns>
        public int GetMaxLimit()
        {
            int i = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.TAKE_PHOTO_COUNT);
            //if(_buyCount == 0)
            //{
            //    return i;
            //}
            //foreach(var v in _takePhotoBuyCountRulePBs)
            //{
            //    if (_buyCount < v.Times)
            //        continue;
            //    i++;
            //}
            return i;
        }

        /// <summary>
        /// 是否能购买
        /// </summary>
        /// <returns></returns>
        public bool isCanBuy()
        {
            var rule = _takePhotoBuyCountRulePBs.Find((m) => { return m.Times == _buyCount + 1; });
            return rule!=null;
        }

        /// <summary>
        /// 购买消耗
        /// </summary>
        /// <returns></returns>
        public int BuyConsume()
        {
            var rule = _takePhotoBuyCountRulePBs.Find((m) => { return m.Times == _buyCount + 1; });
            if(rule==null)
            {
                return 0;
            }
            return rule.Gems;
        }


        public TakePhotoRulePB GetRulePbById(int id)
        {
           return _takePhotoRulePBs.Find((m) => { return m.Id == id; });
        }


    }
}