
#pragma once

#include <windows.h>

namespace utilities
{
	namespace threading
	{
		class CIntraProcessLock
		{
		public:
			CIntraProcessLock()
			{
				InitializeCriticalSection(&m_cs);
			}
			
			~CIntraProcessLock()
			{
				DeleteCriticalSection(&m_cs);
			}

			bool Lock()
			{
				return TryEnterCriticalSection(&m_cs) > 0 ? true : false;
			}

			void Unlock()
			{
				return LeaveCriticalSection(&m_cs);
			}

		private:
			CRITICAL_SECTION m_cs;
		};

		class CNoLock
		{
		public:
			bool Lock() 
			{ 
				return true; 
			}
			
			void Unlock()
			{
				return;
			}
		};
	}
}