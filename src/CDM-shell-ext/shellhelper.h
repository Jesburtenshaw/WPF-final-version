#pragma once

#include "tstring.h"
#include "errorsignaling.h"

namespace utilities
{
	namespace shell
	{
		class helper
		{
		public:

			// Convert SHGDNF flags to string
			static tstring SHGDNF2String(SHGDNF uFlags)
			{
				tstring retVal;

				if (uFlags & SHGDN_INFOLDER)
				{
					retVal += L":INFOLDER:";
				}

				if (uFlags & SHGDN_FORPARSING)
				{
					retVal += L":FORPARSING:";
				}

				if (uFlags & SHGDN_FORADDRESSBAR)
				{
					retVal += L":FORADDRESSBAR:";
				}

				if (uFlags & SHGDN_FOREDITING)
				{
					retVal += L":FOREDITING:";
				}

				return retVal;
			}

			static bool CopyLPCWSTRToLPWSTR(LPCWSTR source, LPWSTR* destination)
			{
				return error_signaling::helper::check_hresult(SHStrDup(source, destination));
			}
		};

	};
};