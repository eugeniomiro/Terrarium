#pragma once


#include "ComWrapper.h"
#include "DWCommon.h"

using namespace System;
using namespace System::Runtime::InteropServices;


namespace Managed { namespace Graphics { namespace DirectWrite 
{
	public ref class GlyphRunAnalysis: ComWrapper
	{
	internal:
		GlyphRunAnalysis(IDWriteGlyphRunAnalysis* native): ComWrapper(native)
		{
		}
	public:
		//TODO: CreateAlphaTexture
		//TODO: GetAlphaBlendParams
		//TODO: GetAlphaTextureBounds
	internal:
		IDWriteGlyphRunAnalysis* GetNative() new 
		{
			return (IDWriteGlyphRunAnalysis*)ComWrapper::GetNative();
		}
	};
}}}