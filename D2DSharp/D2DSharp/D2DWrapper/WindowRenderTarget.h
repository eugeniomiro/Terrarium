/* 
* 
* Authors: 
*  Dmitry Kolchev <dmitrykolchev@msn.com>
*  
*/
#pragma once

#include "D2DCommon.h"
#include "RenderTarget.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Managed { namespace Graphics { namespace Direct2D 
{
	[FlagsAttribute()]
	public enum class WindowState
	{
		None = 0x0000000,
		Occluded = 0x0000001,
	};


	ref class SolidColorBrush;

	public ref class WindowRenderTarget: RenderTarget
	{
	internal:
		WindowRenderTarget(ID2D1HwndRenderTarget* renderTarget): RenderTarget(renderTarget)
		{
		}

	public:
		property WindowState State
		{
			WindowState get()
			{
				int state = GetNative()->CheckWindowState();
				return (WindowState)state;
			}
		}

		property IntPtr Handle
		{
			IntPtr get()
			{
				HWND hwnd = GetNative()->GetHwnd();
				return IntPtr((void*)hwnd);
			}
		}

		void Resize(SizeU size)
		{
			ComUtils::CheckResult(GetNative()->Resize(reinterpret_cast<D2D1_SIZE_U*>(&size)));
		}

	internal:
		ID2D1HwndRenderTarget *GetNative() new
		{
			return reinterpret_cast<ID2D1HwndRenderTarget *>(RenderTarget::GetNative());
		}

	};
}}}