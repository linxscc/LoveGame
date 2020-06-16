using UnityEngine;
using System.Collections;


public static class RectTransformExtension
{
	public static void SetDefaultScale(this RectTransform trans) {
		trans.localScale = new Vector3(1, 1, 1);
	}
	public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
		trans.pivot = aVec;
		trans.anchorMin = aVec;
		trans.anchorMax = aVec;
	}

	public static Vector2 GetSize(this RectTransform trans) {
		return trans.rect.size;
	}
	public static float GetWidth(this RectTransform trans) {
		return trans.rect.width;
	}
	public static float GetHeight(this RectTransform trans) {
		return trans.rect.height;
	}

	public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
	}

	public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}
	public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}
	public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}
	public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}

    public static Vector2 GetLocalCenterPos(this RectTransform trans) {
        float x = trans.localPosition.x - (trans.pivot.x - .5f) * trans.rect.width;
        float y = trans.localPosition.y - (trans.pivot.y - .5f) * trans.rect.height;
        return new Vector3(x, y, trans.localPosition.z);
    }

	public static void SetSize(this RectTransform trans, Vector2 newSize) {
		Vector2 oldSize = trans.rect.size;
		Vector2 deltaSize = newSize - oldSize;
		trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
		trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
	}
	public static void SetWidth(this RectTransform trans, float newSize) {
		SetSize(trans, new Vector2(newSize, trans.rect.size.y));
	}
	public static void SetHeight(this RectTransform trans, float newSize) {
		SetSize(trans, new Vector2(trans.rect.size.x, newSize));
	}
	
	
	public static void CopyRectTransform(this RectTransform rect, RectTransform target)
	{
		rect.anchorMin = new Vector2(target.anchorMin.x, target.anchorMin.y);
		rect.anchorMax = new Vector2(target.anchorMax.x, target.anchorMax.y);
		rect.pivot = new Vector2(target.pivot.x, target.pivot.y);
		rect.anchoredPosition = new Vector2(target.anchoredPosition.x, target.anchoredPosition.y);
		rect.offsetMin = new Vector2(target.offsetMin.x, target.offsetMin.y);
		rect.offsetMax = new Vector2(target.offsetMax.x, target.offsetMax.y);
		rect.sizeDelta = new Vector2(target.sizeDelta.x, target.sizeDelta.y);
        rect.rotation = new Quaternion(target.localRotation.x, target.localRotation.y, target.localRotation.z,target.localRotation.w);
        rect.localScale = new Vector3(target.localScale.x, target.localScale.y, target.localScale.z);
		rect.localEulerAngles=new Vector3(target.localEulerAngles.x,target.localEulerAngles.y,target.localEulerAngles.z);
	}

	public static void SetNormalPivot(this RectTransform rect)
	{
		return;
		Vector2 vector = new Vector2(0.5f, 0.5f);
		rect.pivot = vector;
		rect.offsetMin = vector;
		rect.offsetMax = vector;
	}
}