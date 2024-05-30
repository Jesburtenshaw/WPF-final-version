////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
// FSDrive Shell Namespace Extension
//
// Copyright (c) 2021
//
// Company	: CDM
//
// File     : log.h
//
// Contents	: Definition of the base logger
//
// Author	: Sergey Fasonov
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

#pragma once

#include <iostream>
#include <vector>
#include <Windows.h>
#include "tstring.h"
#include <time.h>

#ifdef UNICODE
#define WRITELOG(logObjPtr, level, fmt, ...) logObjPtr->Log(level, __FILEW__, __LINE__, __FUNCTIONW__, fmt, ##__VA_ARGS__);
#define LOGINFO(logObjPtr, fmt, ...) logObjPtr->Log(utilities::diagnostics::log_level::Info, __FILEW__, __LINE__, __FUNCTIONW__, fmt, ##__VA_ARGS__);
#define LOGDEBUG(logObjPtr, fmt, ...) logObjPtr->Log(utilities::diagnostics::log_level::Debug, __FILEW__, __LINE__, __FUNCTIONW__, fmt, ##__VA_ARGS__);
#define LOGWARN(logObjPtr, fmt, ...) logObjPtr->Log(utilities::diagnostics::log_level::Warn, __FILEW__, __LINE__, __FUNCTIONW__, fmt, ##__VA_ARGS__);
#define LOGERROR(logObjPtr, fmt, ...) logObjPtr->Log(utilities::diagnostics::log_level::Error, __FILEW__, __LINE__, __FUNCTIONW__, fmt, ##__VA_ARGS__);
#else
#define WRITELOG(logObj, level, fmt, ...) logObj.Log(level, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGINFO(logObj, fmt, ...) logObj.Log(framework::Diagnostics::LogLevel::Info, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGDEBUG(logObj, fmt, ...) logObj.Log(framework::Diagnostics::LogLevel::Debug, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGINFO(logObj, fmt, ...) logObj.Log(framework::Diagnostics::LogLevel::Warn, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGERROR(logObj, fmt, ...) logObj.Log(framework::Diagnostics::LogLevel::Error, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);

#define WRITELOGP(logObjPtr, level, fmt, ...) logObjPtr->Log(level, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGINFOP(logObjPtr, fmt, ...) logObjPtr->Log(framework::Diagnostics::LogLevel::Info, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGDEBUGP(logObjPtr, fmt, ...) logObjPtr->Log(framework::Diagnostics::LogLevel::Debug, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGINFOP(logObjPtr, fmt, ...) logObjPtr->Log(framework::Diagnostics::LogLevel::Warn, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#define LOGERRORP(logObjPtr, fmt, ...) logObjPtr->Log(framework::Diagnostics::LogLevel::Error, __FILE__, __LINE__, __FUNCTION__, fmt, ##__VA_ARGS__);
#endif

#define MAX_TEXT_BUFFER_SIZE 512

namespace utilities
{
	namespace diagnostics
	{
		enum class log_level
		{
			Info,
			Debug,
			Warn,
			Error
		};

		enum class log_item
		{
			Filename = 0x1,
			LineNumber = 0x2,
			Function = 0x4,
			DateTime = 0x8,
			ThreadId = 0x10,
			LoggerName = 0x20,
			LogLevel = 0x40
		};

		template <class ThreadingProtection> class CLogger
		{
			private:
				struct StreamInfo
				{
					tostream* pStream;
					bool owned;
					log_level level;

					StreamInfo(tostream* pStream, const bool owned, const log_level level)
					{
						this->pStream = pStream;
						this->owned = owned;
						this->level = level;
					}
				};

			public:
				CLogger(const diagnostics::log_level level, LPCTSTR name, const int loggableItems = static_cast<int>(log_item::Function) | static_cast<int>(log_item::LineNumber) | static_cast<int>(log_item::DateTime) |
						static_cast<int>(log_item::LoggerName) | static_cast<int>(log_item::LogLevel))
					: m_loggableItem(loggableItems), m_level(level), m_name(name)
				{
				}

				~CLogger()
				{
					ClearOutputStreams();
				}
				
				void AddOutputStream(tostream* os, bool own)
				{
					AddOutputStream(os, own, m_level);
				}

				void Log(log_level level, LPCTSTR file, const INT line, LPCTSTR func, LPCTSTR fmt, ...)
				{
					if (!m_threadProtect.Lock()) 
						return;

					TCHAR text[MAX_TEXT_BUFFER_SIZE] = { 0 };
					va_list args;

					va_start(args, fmt);
					_vstprintf_s(text, MAX_TEXT_BUFFER_SIZE, fmt, args);
					va_end(args);

					for (vector<StreamInfo>::iterator iter = m_outputStreams.begin(); iter < m_outputStreams.end(); iter++)
					{
						if (level < iter->level)
						{
							continue;
						}

						bool written = false;
						tostream * pStream = iter->pStream;

						if (m_loggableItem & static_cast<int>(log_item::DateTime))
							written = write_datetime(written, pStream);

						if (m_loggableItem & static_cast<int>(log_item::ThreadId))
							written = write<int>(GetCurrentThreadId(), written, pStream);

						if (m_loggableItem & static_cast<int>(log_item::LoggerName))
							written = write<LPCTSTR>(m_name.c_str(), written, pStream);

						if (m_loggableItem & static_cast<int>(log_item::LogLevel))
						{
							written = write<LPCTSTR>(log_level_to_string(level).c_str(), written, pStream);
						}

						if (m_loggableItem & static_cast<int>(log_item::Function))
							written = write<LPCTSTR>(func, written, pStream);

						if (m_loggableItem & static_cast<int>(log_item::Filename))
							written = write<LPCTSTR>(file, written, pStream);

						if (m_loggableItem & static_cast<int>(log_item::LineNumber))
							written = write<int>(line, written, pStream);

						written = write<LPCTSTR>(text, written, pStream);

						if (written)
						{
							(*pStream) << endl;
							pStream->flush();
						}
					}

					m_threadProtect.Unlock();
				}

			private:
				int m_loggableItem;
				log_level m_level;
				tstring m_name;
				vector<StreamInfo> m_outputStreams;
				ThreadingProtection m_threadProtect;

				void AddOutputStream(tostream& os, bool own, log_level level)
				{
					AddOutputStream(&os, own, level);
				}

				void AddOutputStream(tostream& os, bool own)
				{
					AddOutputStream(os, own, m_level);
				}

				void AddOutputStream(tostream* os, bool own, log_level level)
				{
					StreamInfo si(os, own, level);
					m_outputStreams.push_back(si);
				}

				void ClearOutputStreams()
				{
					for (vector<StreamInfo>::iterator iter = m_outputStreams.begin(); iter < m_outputStreams.end(); iter++)
					{
						if (iter->owned) delete iter->pStream;
					}

					m_outputStreams.clear();
				}

				template <class T> bool write(T data, const bool written, tostream* strm)
				{
					if (written == true)
					{
						(*strm) << _T(" ");
					}

					(*strm) << data;
					return true;
				}

				bool write_datetime(const bool written, tostream* strm) const
				{
					if (written == true)
					{
						(*strm) << _T(" ");
					}

					time_t szClock;
					tm newTime;

					time(&szClock);
					localtime_s(&newTime, &szClock);

					TCHAR strDate[10] = { _T('\0') };
					TCHAR strTime[10] = { _T('\0') };

					_tstrdate_s(strDate, 10);
					_tstrtime_s(strTime, 10);

					(*strm) << strDate << _T(" ") << strTime;

					return true;
				}

				tstring log_level_to_string(const log_level level) const
				{
					switch (level)
					{
					case log_level::Error:
						return tstring(_T("[ERROR]"));

					case log_level::Warn:
						return tstring(_T("[WARNING]"));

					case log_level::Info:
						return tstring(_T("[INFO]"));

					case log_level::Debug:
						return tstring(_T("[DEBUG]"));

					default:
						return tstring(_T("[UNKNOWN]"));
					}
				}
		};
	}
}