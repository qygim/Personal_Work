using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

class Image
{
	// 그림 그리는 구조체
	struct RenderChar
	{
		public char c;
		public ConsoleColor co;
		public int _x;
		public int _y;
	}

	// 컬러 -> 콘솔컬러
	public static System.ConsoleColor FromColor(System.Drawing.Color c)
	{
		int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
		index |= (c.R > 64) ? 4 : 0; // Red bit
		index |= (c.G > 64) ? 2 : 0; // Green bit
		index |= (c.B > 64) ? 1 : 0; // Blue bit
		return (System.ConsoleColor)index;
	}

	const int nCONSOLE_WIDTH = 150;
	const int nCONSOLE_HEIGHT = 40;
	const string sGrayScale = "*";
	readonly static int nScale;

	static List<RenderChar> m_liRender = new List<RenderChar>();

	static Image() { nScale = sGrayScale.Length; }

	public void Draw(string[] args,int SinnerID)
	{
		// arg 체크
		if (args.Length == 0)
		{
			Console.WriteLine("no file name");
			return;
		}

		// 콘솔 사이즈 : 100 / 40 을 맥스 사이즈로
		Console.WindowWidth = nCONSOLE_WIDTH + 20;
		Console.WindowHeight = nCONSOLE_HEIGHT + 5;
		Console.CursorVisible = false;

		// 이미지 로딩
		Bitmap bitmap = null;
		SinnerID = SinnerID % 101;
		try
		{
			bitmap = new Bitmap(args[SinnerID]);
			var size = bitmap.Size;

			int nDrawWidth = size.Width;
			int nDrawHeight = size.Height;
			float fPixelJumpRatioX = 1;
			float fPixelJumpRatioY = 1;

			// 이미지가 콘솔창보다 큰 부분이 존재
			if (size.Width > nCONSOLE_WIDTH || size.Height > nCONSOLE_HEIGHT)
			{
				int nGCD = GetGCD(size.Width, size.Height);
				float fRatioX = size.Width / nGCD;
				float fRatioY = size.Height / nGCD;

				if (fRatioX > fRatioY)
				{
					nDrawWidth = nCONSOLE_WIDTH;
					nDrawHeight = (int)(size.Height * (nCONSOLE_WIDTH / (float)size.Width));
				}
				else
				{
					nDrawWidth = (int)(size.Width * (nCONSOLE_HEIGHT / (float)size.Height));
					nDrawHeight = nCONSOLE_HEIGHT;
				}

				if (size.Width > nCONSOLE_WIDTH)
				{
					fPixelJumpRatioX = (float)size.Width / nDrawWidth;
				}

				if (size.Height > nCONSOLE_HEIGHT)
				{
					fPixelJumpRatioY = (float)size.Height / nDrawHeight;
				}
			}

			m_liRender.Capacity = nDrawWidth * nDrawHeight;

			for (int y = 0; y < nDrawHeight; ++y)
			{
				for (int x = 0; x < nDrawWidth; ++x)
				{
					int nIndex = x + y * nDrawWidth;

					float fPixelX = x * fPixelJumpRatioX;
					float fPixelY = y * fPixelJumpRatioY;

					Color color = bitmap.GetPixel((int)fPixelX, (int)fPixelY);

					float fBright = (0.3f * (color.R / (float)255)) +
						(0.59f * (color.G / (float)255)) +
						(0.11f * (color.B / (float)255));

					if (color.A > 10)
					{
						fBright *= (color.A / (float)255);
					}

					int nScaleIndex = nScale - (int)((fBright * nScale));

					// Clamp
					if (nScaleIndex >= nScale) { nScaleIndex = nScale - 1; }
					if (nScaleIndex < 0) { nScaleIndex = 0; }

					m_liRender.Add(new RenderChar()
					{
						c = sGrayScale[nScaleIndex],
						co = FromColor(color),
						_x = x,
						_y = y
					});
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("error occured - file check");
			Console.WriteLine(e.Message);
			return;
		}
		finally
		{
			bitmap.Dispose();
		}

		// 그림
		foreach (var val in m_liRender)
		{
			Console.SetCursorPosition(val._x + 6, val._y + 4);
			Console.ForegroundColor = val.co;
			Console.Write(val.c);
		}
		Console.WriteLine(args[SinnerID].ToString());
		//Console.Read();
	}

	static int GetGCD(int x, int y)
	{
		if (x == 0)
		{
			return y;
		}

		return GetGCD(y % x, x);
	}
}

