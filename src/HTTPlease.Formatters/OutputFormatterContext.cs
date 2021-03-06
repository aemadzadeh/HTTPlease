﻿using System;
using System.Text;

namespace HTTPlease.Formatters
{
	using System.IO;

	/// <summary>
	///		Contextual information used by output formatters.
	/// </summary>
	public class OutputFormatterContext
	{
		/// <summary>
		///		Create a new <see cref="OutputFormatterContext"/>.
		/// </summary>
		/// <param name="data">
		///		The data being serialised.
		/// </param>
		/// <param name="dataType">
		///		The CLR type whose data will be serialised.
		/// </param>
		/// <param name="mediaType">
		///		The media type that the formatter should produced.
		/// </param>
		/// <param name="encoding">
		///		The content encoding that the formatter should use.
		/// </param>
		public OutputFormatterContext(object data, Type dataType, string mediaType, Encoding encoding)
		{
			Data = data;
			DataType = dataType;
			MediaType = mediaType;
			Encoding = encoding;
		}

		/// <summary>
		///		The data being serialised.
		/// </summary>
		public object Data { get; }

		/// <summary>
		///		The CLR type whose data will be serialised.
		/// </summary>
		public Type DataType { get; }

		/// <summary>
		///		The content type being serialised.
		/// </summary>
		public string MediaType { get; }

		/// <summary>
		///		The content encoding.
		/// </summary>
		public Encoding Encoding { get; }

		/// <summary>
		///		Create a <see cref="TextWriter"/> from the specified output stream.
		/// </summary>
		/// <param name="outputStream">
		///		The output stream.
		/// </param>
		/// <returns>
		///		The <see cref="TextWriter"/>.
		/// </returns>
		/// <remarks>
		///		The <see cref="TextWriter"/>, when closed, will not close the output stream.
		/// </remarks>
		public virtual TextWriter CreateWriter(Stream outputStream)
		{
			if (outputStream == null)
				throw new ArgumentNullException(nameof(outputStream));

			return StreamHelper.CreateTransientTextWriter(outputStream, Encoding);
		}
	}
}