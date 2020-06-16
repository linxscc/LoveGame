using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Componets
{
    public class LoopCarouselImage : UIBehaviour, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, ICanvasElement
    {
    

        /// <summary>
        /// 滚动方向H or V
        /// </summary>
        public enum AxisType
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// 图片轮播方向
        /// </summary>
        public enum LoopDirType
        {
            RightOrUp = -1,
            LeftOrDown = 1,
        }

        /// <summary>
        /// 子物体size
        /// </summary>
        public Vector2 mCellSize;

        /// <summary>
        /// 子物体间隔
        /// </summary>
        public Vector2 mSpacing;

        /// <summary>
        /// 方向
        /// </summary>
        public AxisType MMoveAxisType;

        /// <summary>
        /// 轮播方向-- 1为向左移动，-1为向右移动
        /// </summary>
        public LoopDirType mLoopDirType = LoopDirType.LeftOrDown;

        /// <summary>
        /// Tween时的步数
        /// </summary>
        [Range(1, 500)]
        public int mTweenStepNum = 150;

        /// <summary>
        /// 自动轮播
        /// </summary>
        public bool mAutoLoop = false;

        /// <summary>
        /// 可否拖动
        /// </summary>
        public bool mDrag = false;

        /// <summary>
        /// 下一次播放间隔时间
        /// </summary>
        public float mLoopSpaceTime = 1;


        public bool AddChildOver = false;
        private bool isAddChild = true;

        /// <summary>
        /// 当前处于正中的元素
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return m_index;
            }

        }

        private bool m_Dragging = false;
        private bool m_IsNormalizing = false;
        private Vector2 m_CurrentPos;
        private int m_currentStep = 0;
        private RectTransform viewRectTran;
        private Vector2 m_PrePos;
        private int m_index = 0;
        private RectTransform header;
        private bool contentCheckCache = true;
        private bool moveOver = true;
        private float currTimeDelta = 0;
        private float viewRectXMin
        {
            get
            {
                Vector3[] v = new Vector3[4];
                viewRectTran.GetWorldCorners(v);
                return v[0].x;
            }
        }
        private float viewRectXMax
        {
            get
            {
                Vector3[] v = new Vector3[4];
                viewRectTran.GetWorldCorners(v);
                return v[3].x;
            }
        }
        private float viewRectYMin
        {
            get
            {
                Vector3[] v = new Vector3[4];
                viewRectTran.GetWorldCorners(v);
                return v[0].y;
            }
        }
        private float viewRectYMax
        {
            get
            {
                Vector3[] v = new Vector3[4];
                viewRectTran.GetWorldCorners(v);
                return v[2].y;
            }
        }
        

        public int CellCount
        {
            get
            {              
                return transform.childCount;
            }
            
        }

      

        public ToggleGroup ToggleGroup;


        private List<Toggle> _toggleList;



        protected override void Awake()
        {
            base.Awake();
            viewRectTran = GetComponent<RectTransform>();

            if (ToggleGroup != null)
            {
                _toggleList = new List<Toggle>();
            }
        }



        public void ResizeChildren()
        {
            Vector2 delta;
            if (MMoveAxisType == AxisType.Horizontal)
            {
                delta = new Vector2(mCellSize.x + mSpacing.x, 0);
            }
            else
            {
                delta = new Vector2(0, mCellSize.y + mSpacing.y);
            }
            for (int i = 0; i < CellCount; i++)
            {
                var t = GetChild(viewRectTran, i);
                if (t)
                {
                    t.localPosition = delta * i;
                    t.sizeDelta = mCellSize;
                }
            }
            m_IsNormalizing = false;
            m_CurrentPos = Vector2.zero;
            m_currentStep = 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ResizeChildren();
            return;

        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }

        protected virtual void Update()
        {
            if (AddChildOver)
            {
                if (isAddChild)
                {
                    header = GetChild(viewRectTran, 0);

                    for (int i = 0; i < ToggleGroup.transform.childCount; i++)
                    {
                        var t = ToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
                        _toggleList.Add(t);
                    }

                  
                    isAddChild = false;
                }

                if (ContentIsLongerThanRect())
                {
                    //实现在必要时loop子元素
                    if (Application.isPlaying)
                    {
                        int s = GetBoundaryState();
                        LoopCell(s);
                    }
                    //缓动回指定位置
                    if (m_IsNormalizing && EnsureListCanAdjust())
                    {
                        if (m_currentStep == mTweenStepNum)
                        {
                            m_IsNormalizing = false;
                            m_currentStep = 0;
                            m_CurrentPos = Vector2.zero;
                            return;
                        }
                        Vector2 delta = m_CurrentPos / mTweenStepNum;
                        m_currentStep++;
                        TweenToCorrect(-delta);
                        moveOver = false;
                    }
                    //自动loop
                    if (mAutoLoop && !m_IsNormalizing && EnsureListCanAdjust())
                    {
                        moveOver = true;
                        currTimeDelta += Time.deltaTime;
                        if (currTimeDelta > mLoopSpaceTime)
                        {
                            currTimeDelta = 0;
                            MoveToIndex(m_index + (int)mLoopDirType);
                        }
                    }
                    //检测index是否变化
                    if (MMoveAxisType == AxisType.Horizontal)
                    {
                        m_index = (int)(header.localPosition.x / (mCellSize.x + mSpacing.x - 1));

                    }
                    else
                    {
                        m_index = (int)(header.localPosition.y / (mCellSize.y + mSpacing.y - 1));
                    }
                    if (m_index <= 0)
                    {
                        m_index = Mathf.Abs(m_index);

                    }
                    else
                    {
                        m_index = CellCount - m_index;

                    }
                    if (moveOver)
                    {
                        _toggleList[m_index].isOn = true;
                    }

                }
            }


        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!mDrag || !contentCheckCache)
            {
                return;
            }
            Vector2 vector;
            if (((eventData.button == PointerEventData.InputButton.Left) && this.IsActive()) && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRectTran, eventData.position, eventData.pressEventCamera, out vector))
            {
                this.m_Dragging = true;
                m_PrePos = vector;
            }
            moveOver = false;
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (!mDrag)
            {
                return;
            }
            return;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!mDrag || !contentCheckCache)
            {
                return;
            }
            Vector2 vector;
            if (((eventData.button == PointerEventData.InputButton.Left) && this.IsActive()) && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRectTran, eventData.position, eventData.pressEventCamera, out vector))
            {
                m_IsNormalizing = false;
                m_CurrentPos = Vector2.zero;
                m_currentStep = 0;
                Vector2 vector2 = vector - this.m_PrePos;
                Vector2 vec = CalculateOffset(vector2);
                this.SetContentPosition(vec);
                m_PrePos = vector;
            }
            moveOver = false;
        }
        /// <summary>
        /// 移动到指定索引
        /// </summary>
        /// <param name="ind"></param>
        public virtual void MoveToIndex(int ind)
        {
            if (m_IsNormalizing)
            {
                return;
            }
            if (ind == m_index)
            {
                return;
            }
            this.m_IsNormalizing = true;
            Vector2 offset;
            if (MMoveAxisType == AxisType.Horizontal)
            {
                offset = new Vector2(mCellSize.x + mSpacing.x, 0);
            }
            else
            {
                offset = new Vector2(0, mCellSize.y + mSpacing.y);
            }
            var delta = CalcCorrectDeltaPos();
            int vindex = m_index;
            m_CurrentPos = delta + offset * (ind - vindex);
            m_currentStep = 0;
        }
        private Vector2 CalculateOffset(Vector2 delta)
        {
            if (MMoveAxisType == AxisType.Horizontal)
            {
                delta.y = 0;
            }
            else
            {
                delta.x = 0;
            }
            return delta;
        }
        private void SetContentPosition(Vector2 position)
        {
            foreach (RectTransform i in viewRectTran)
            {
                i.localPosition += (Vector3)position;
            }
            return;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!mDrag || !contentCheckCache)
            {
                return;
            }
            this.m_Dragging = false;
            this.m_IsNormalizing = true;
            m_CurrentPos = CalcCorrectDeltaPos();
            m_currentStep = 0;
            moveOver = false;
        }

        public virtual void Rebuild(CanvasUpdate executing)
        {
            return;
        }
        /// <summary>
        /// List是否处于可自由调整状态
        /// </summary>
        /// <returns></returns>
        public virtual bool EnsureListCanAdjust()
        {
            return !m_Dragging && ContentIsLongerThanRect();
        }
        /// <summary>
        /// 内容是否比显示范围大
        /// </summary>
        /// <returns></returns>
        public virtual bool ContentIsLongerThanRect()
        {
            float contentLen;
            float rectLen;
            if (MMoveAxisType == AxisType.Horizontal)
            {
                contentLen = CellCount * (mCellSize.x + mSpacing.x) - mSpacing.x;
                rectLen = viewRectTran.rect.xMax - viewRectTran.rect.xMin;
            }
            else
            {
                contentLen = CellCount * (mCellSize.y + mSpacing.y) - mSpacing.y;
                rectLen = viewRectTran.rect.yMax - viewRectTran.rect.yMin;
            }
            contentCheckCache = contentLen > rectLen;
            return contentCheckCache;
        }
        /// <summary>
        /// 检测边界情况，分为0未触界，-1左(下)触界，1右(上)触界
        /// </summary>
        /// <returns></returns>
        public virtual int GetBoundaryState()
        {
            RectTransform left;
            RectTransform right;
            left = GetChild(viewRectTran, 0);
            right = GetChild(viewRectTran, CellCount - 1);
            Vector3[] l = new Vector3[4];
            left.GetWorldCorners(l);
            Vector3[] r = new Vector3[4];
            right.GetWorldCorners(r);
            if (MMoveAxisType == AxisType.Horizontal)
            {
                if (l[0].x >= viewRectXMin)
                {
                    return -1;
                }
                else if (r[3].x < viewRectXMax)
                {
                    return 1;
                }
            }
            else
            {
                if (l[0].y >= viewRectYMin)
                {
                    return -1;
                }
                else if (r[1].y < viewRectYMax)
                {
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// Loop列表，分为-1把最右(上)边一个移到最左(下)边，1把最左(下)边一个移到最右(上)边
        /// </summary>
        /// <param name="dir"></param>
        protected virtual void LoopCell(int dir)
        {
            if (dir == 0)
            {
                return;
            }
            RectTransform MoveCell;
            RectTransform Tarborder;
            Vector2 TarPos;
            if (dir == 1)
            {
                MoveCell = GetChild(viewRectTran, 0);
                Tarborder = GetChild(viewRectTran, CellCount - 1);
                MoveCell.SetSiblingIndex(CellCount - 1);
            }
            else
            {
                Tarborder = GetChild(viewRectTran, 0);
                MoveCell = GetChild(viewRectTran, CellCount - 1);
                MoveCell.SetSiblingIndex(0);
            }
            if (MMoveAxisType == AxisType.Horizontal)
            {
                TarPos = Tarborder.localPosition + new Vector3((mCellSize.x + mSpacing.x) * dir, 0, 0);
            }
            else
            {
                TarPos = (Vector2)Tarborder.localPosition + new Vector2(0, (mCellSize.y + mSpacing.y) * dir);
            }
            MoveCell.localPosition = TarPos;
        }
        /// <summary>
        /// 计算一个最近的正确位置
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 CalcCorrectDeltaPos()
        {
            Vector2 delta = Vector2.zero;
            float distance = float.MaxValue;
            foreach (RectTransform i in viewRectTran)
            {
                var td = Mathf.Abs(i.localPosition.x) + Mathf.Abs(i.localPosition.y);
                if (td <= distance)
                {
                    distance = td;
                    delta = i.localPosition;
                }
                else
                {
                    break;
                }
            }
            return delta;
        }
        /// <summary>
        /// 移动指定增量
        /// </summary>
        protected virtual void TweenToCorrect(Vector2 delta)
        {
            foreach (RectTransform i in viewRectTran)
            {
                i.localPosition += (Vector3)delta;
            }
        }

        private static RectTransform GetChild(RectTransform parent, int index)
        {
            if (parent == null || index >= parent.childCount)
            {
                return null;
            }
            return parent.GetChild(index) as RectTransform;
        }

        public void LayoutComplete()
        {
        }

        public void GraphicUpdateComplete()
        {
        }

        /// <summary>
        /// 重置循环（让其从头开始，不删除）
        /// </summary>
        public void ResetLoop()
        {
            m_index = 0;
        
            ToggleGroup.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
            List<string> temp = new List<string>();
            var viewRectTran = GetComponent<RectTransform>();

            for (int i = 0; i < viewRectTran.childCount; i++)
            {
                if (viewRectTran.GetChild(i).gameObject)
                {
                    temp.Add(viewRectTran.GetChild(i).gameObject.name);
                }
                       
            }
            temp.Sort();
           
            for (int i = 0; i < temp.Count; i++)
            {
                var item = viewRectTran.Find(temp[i]).gameObject;
                if (MMoveAxisType == AxisType.Horizontal)
                {
                    item.transform.localPosition = new Vector2(i * (mCellSize.x + mSpacing.x), 0);
                }
                else
                {
                    item.transform.localPosition = new Vector2(0, i * (mCellSize.y + mSpacing.y));
                }

                item.GetComponent<RectTransform>().SetSiblingIndex(i);
            }

        }



           /// <summary>
           /// 刷新（会把之前的数据删掉）
           /// </summary>
        public void RefreshLoop()
        {
            enabled = false;
            for (int i = 0; i < CellCount; i++)
            {
                Destroy(viewRectTran.GetChild(i).gameObject);
                Destroy(ToggleGroup.transform.GetChild(i).gameObject);
            }
            enabled = true;

            _toggleList.Clear();
            m_index = 0;       
            isAddChild = true;
            header = GetChild(viewRectTran, 0);

        }




}
}

