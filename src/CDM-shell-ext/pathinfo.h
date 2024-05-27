///////////////////
#pragma once

#include "tstring.h"

namespace utilities
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////   
	// path_info - wrapper class to access path functions
	class path_info
	{
		public:

			static tstring get_dll_folder(HINSTANCE hInstance)
			{
				// Get the module filename
				TCHAR szModule[MAX_PATH + 1];
				DWORD dwFLen = ::GetModuleFileName(hInstance, szModule, MAX_PATH);
				if (dwFLen == 0)
					return tstring(_T(""));

				// Get the full path to the shim DLL, or bail.
				TCHAR szPath[MAX_PATH + 1];
				DWORD dwPathBufferSize = sizeof(szPath) / sizeof(szPath[0]);
				TCHAR* pszFileName;
				dwFLen = ::GetFullPathName(szModule, dwPathBufferSize, szPath, &pszFileName);
				if (dwFLen == 0 || dwFLen >= dwPathBufferSize)
					return tstring(_T(""));

				*pszFileName = 0;
				return tstring(szPath);
			}

			static bool file_exists(const tstring& path)
			{
				if (path.length() == 0)
					return false;

				if (_waccess(path.c_str(), 0) == 0)
				{
					return true;
				}

				return false;
			}

			static tstring combine_path(const tstring& path1, const tstring& path2)
			{
				tstring path;

				if (path1.length() > 0 && path2.length() > 0)
				{
					path = path1 + path2;
				}
				else if (path1.length() > 0)
				{
					path = path1;
				}
				else if (path2.length() > 0)
				{
					path = path2;
				}

				return path;
			}

			static tstring get_file_url(const tstring& path)
			{
				tstring url = path;

				size_t start_index = tstring::npos;

				while (start_index < url.length())
				{
					size_t index = url.find(L"\\", start_index);

					if (index == tstring::npos)
						break;

					url.replace(index, 1, L"/");

					start_index = index + 1;
				}

				if (url.length() > 0)
				{
					url = L"file:///" + url;
				}

				return url;
			}
			
	};
};
