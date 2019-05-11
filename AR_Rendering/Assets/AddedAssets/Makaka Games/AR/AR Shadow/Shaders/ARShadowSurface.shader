﻿/*
============================
Unity Assets by MAKAKA GAMES
============================

Online Docs: https://makaka.org/unity-assets
Offline Docs: You have a PDF file in the package folder.

=======
SUPPORT
=======

First of all, read the docs. If it didn’t help, get the support.

Web: https://makaka.org/support
Email: info@makaka.org

If you find a bug or you can’t use the asset as you need, 
please first send email to info@makaka.org (in English or in Russian) 
before leaving a review to the asset store.

I am here to help you and to improve my products for the best.
*/

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AR/ARShadowSurface" 
{
	Properties
	{ 
		_Color("Main Color", Color) = (1,1,1,1) 
		_MainTex("Base (RGB)", 2D) = "white" {}	
		_Cutoff("Cutout", Range(0,1)) = 0.5 
	}

	SubShader
	{ 
		Pass 
		{ 
			Alphatest Greater[_Cutoff] 
			SetTexture[_MainTex] 
		}	

		Pass
		{ 
			Blend DstColor Zero Tags 
			{ 
				"LightMode" = "ForwardBase" 
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"

			struct v2f 
			{ 
				float4 pos : SV_POSITION; 
				LIGHTING_COORDS(0,1) 
			};

			v2f vert(appdata_base v) 
			{
				v2f o; 
				o.pos = UnityObjectToClipPos(v.vertex); 
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o; 
			}

			fixed4 frag(v2f i) : COLOR
			{
				float attenuation = LIGHT_ATTENUATION(i);
				return attenuation;
			} 

			ENDCG 
		} 
	} 

	Fallback "Transparent/Cutout/VertexLit" 
} 