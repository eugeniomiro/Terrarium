#include "Stdafx.h"

namespace Managed { namespace Graphics { namespace DirectWrite
{


Managed::Graphics::DirectWrite::FontCollection^ TextFormat::FontCollection::get()
{
	IDWriteFontCollection* value;
	ComUtils::CheckResult(GetNative()->GetFontCollection(&value));
	return gcnew Managed::Graphics::DirectWrite::FontCollection(value);
}

String^ TextFormat::FontFamilyName::get()
{
	IDWriteTextFormat* native = GetNative();
	UINT len = native->GetFontFamilyNameLength();
	wchar_t *name = new wchar_t[len + 1];
	try
	{
		ComUtils::CheckResult(native->GetFontFamilyName(name, len + 1));
		return gcnew String(name);
	}
	finally
	{
		delete name;
	}
}

Single TextFormat::FontSize::get()
{
	return GetNative()->GetFontSize();
}

Managed::Graphics::DirectWrite::FontStretch TextFormat::FontStretch::get()
{
	return (Managed::Graphics::DirectWrite::FontStretch)GetNative()->GetFontStretch();
}

Managed::Graphics::DirectWrite::FontStyle TextFormat::FontStyle::get()
{
	return (Managed::Graphics::DirectWrite::FontStyle)GetNative()->GetFontStyle();
}

Managed::Graphics::DirectWrite::FontWeight TextFormat::FontWeight::get()
{
	return (Managed::Graphics::DirectWrite::FontWeight)GetNative()->GetFontWeight();
}


Managed::Graphics::DirectWrite::LineSpacing TextFormat::LineSpacing::get()
{
	DWRITE_LINE_SPACING_METHOD method;
	FLOAT height;
	FLOAT baseLine;
	ComUtils::CheckResult(GetNative()->GetLineSpacing(&method, &height, &baseLine));
	return Managed::Graphics::DirectWrite::LineSpacing((LineSpacingMethod)method, height, baseLine);
}

void TextFormat::LineSpacing::set(Managed::Graphics::DirectWrite::LineSpacing value)
{
	ComUtils::CheckResult(GetNative()->SetLineSpacing((DWRITE_LINE_SPACING_METHOD)value.Method, value.Height, value.BaseLine));
}


System::Globalization::CultureInfo^ TextFormat::Culture::get()
{
	IDWriteTextFormat* native = GetNative();
	UINT32 len = native->GetLocaleNameLength();
	wchar_t *name = new wchar_t[len + 1];
	try
	{
		ComUtils::CheckResult(native->GetLocaleName(name, len + 1));
		return gcnew System::Globalization::CultureInfo(gcnew String(name));
	}
	finally
	{
		delete name;
	}
}


Managed::Graphics::DirectWrite::Trimming TextFormat::Trimming::get()
{
	Managed::Graphics::DirectWrite::Trimming value;
	IDWriteInlineObject* inlineObject = NULL;
	try
	{
		ComUtils::CheckResult(GetNative()->GetTrimming((DWRITE_TRIMMING*)&value, &inlineObject));
		return value;
	}
	finally
	{
		if(inlineObject != NULL)
			inlineObject->Release();
	}
}

void TextFormat::Trimming::set(Managed::Graphics::DirectWrite::Trimming value)
{
	ComUtils::CheckResult(GetNative()->SetTrimming((DWRITE_TRIMMING*)&value, NULL));
}


Managed::Graphics::DirectWrite::WordWrapping TextFormat::WordWrapping::get()
{
	return (Managed::Graphics::DirectWrite::WordWrapping)GetNative()->GetWordWrapping();
}

void TextFormat::WordWrapping::set(Managed::Graphics::DirectWrite::WordWrapping value)
{
	ComUtils::CheckResult(GetNative()->SetWordWrapping((DWRITE_WORD_WRAPPING)value));
}

}}}