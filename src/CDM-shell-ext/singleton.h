
#pragma once

namespace utilities
{
	template <typename T, typename CONTEXT>
	class Singleton
	{
	public:
		T* operator->() { return m_pInstance; }

		const T* operator->() const { return m_pInstance; }

		T& operator*() { return *m_pInstance; }

		const T& operator*() const { return *m_pInstance; }

	protected:
		Singleton()
		{
			static bool static_init = []()->bool {
				m_pInstance = new T;
				return true;
			}();
		}

		Singleton(int)
		{
			static bool static_init = []()->bool {
				m_pInstance = CONTEXT::init();
				return true;
			}();
		}

	private:
		static T* m_pInstance;
	};

	template <typename T, typename CONTEXT>
	T *Singleton<T, CONTEXT>::m_pInstance;

}