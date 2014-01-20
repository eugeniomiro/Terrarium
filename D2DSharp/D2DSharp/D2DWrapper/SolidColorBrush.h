/* 
* 
* Authors: 
*  Dmitry Kolchev <dmitrykolchev@msn.com>
*  
*/
#pragma once

#include "D2DCommon.h"
#include "Brush.h"
#include "Color.h"

using namespace System;

namespace Managed { namespace Graphics { namespace Direct2D 
{
	public ref class SolidColorBrush: Brush
	{
	internal:
		SolidColorBrush(ID2D1SolidColorBrush* brush): 
			Brush(brush)
		{
		}
	public:
		property Managed::Graphics::Direct2D::Color Color
		{
			Managed::Graphics::Direct2D::Color get()
			{
				return Managed::Graphics::Direct2D::Color(GetNative()->GetColor());
			}
			void set(Managed::Graphics::Direct2D::Color color)
			{
				GetNative()->SetColor((D2D1_COLOR_F*)&color);
			}
		}
	private:
		ID2D1SolidColorBrush* GetNative() new
		{
			return (ID2D1SolidColorBrush*)Brush::GetNative();
		}
	};
}}}