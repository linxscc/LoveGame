/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System.Collections.Generic;

interface ModelSetting 
{
	

	
	string GetModelName()		 ;
	string GetModelFile()		 ;

	
	int GetTextureNum()			 ;
	string GetTextureFile(int n) ;
	string[] GetTextureFiles() ;

	
	int GetHitAreasNum()		;
	string GetHitAreaID(int n)	;
	string GetHitAreaName(int n);

	
	string GetPhysicsFile()	;
	string GetPoseFile() ;
	int GetExpressionNum() ;
	string GetExpressionFile(int n) ;
	string[] GetExpressionFiles() ;
	string GetExpressionName(int n) ;
	string[] GetExpressionNames() ;

	
	string[] GetMotionGroupNames()	;
	int GetMotionNum(string name)	;

	string GetMotionFile(string name,int n) ;
	string GetMotionSound(string name,int n) ;
	int GetMotionFadeIn(string name,int n) ;
	int GetMotionFadeOut(string name,int n) ;

	
	bool GetLayout(Dictionary<string, float> layout) ;
	
	
	int GetInitParamNum();
	float GetInitParamValue(int n);
	string GetInitParamID(int n);

	
	int GetInitPartsVisibleNum();
	float GetInitPartsVisibleValue(int n);
	string GetInitPartsVisibleID(int n);
	
}