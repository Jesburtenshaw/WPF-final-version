////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
// FSDrive Shell Namespace Extension
//
// Copyright (c) 2021
//
// Company	: Tierfive
//
// File     : errorsignaling.h
//
// Contents	: Definition of the helpers functions to check errors - hresults and codes
//
// Author	: Sergey Fasonov
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

#pragma once

#include "application.h"

namespace utilities
{
	namespace error_signaling
	{
		class helper
		{
		public:

			static bool check_hresult(HRESULT hr)
			{
				if (!SUCCEEDED(hr))
				{
					LOGERROR(_MainApplication->GetLogger(), L"check_hresult: HRESULT 0x%08X is failed", hr);
					return false;
				}

				return true;
			}

			static bool check_hresult_retval(HRESULT hr)
			{
				if (!SUCCEEDED(hr))
				{
					LOGERROR(_MainApplication->GetLogger(), L"check_hresult_retval: HRESULT 0x%08X is failed", hr);
				}

				return hr;
			}
		};
	};
};
