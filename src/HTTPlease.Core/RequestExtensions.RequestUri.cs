﻿using System;

namespace HTTPlease
{
	using Core.Utilities;

	/// <summary>
	///		<see cref="HttpRequest"/> / <see cref="IHttpRequest"/> extension methods for request URIs.
	/// </summary>
	public static partial class RequestExtensions
    {
		/// <summary>
		///		Ensure that the <see cref="IHttpRequest"/> has an <see cref="UriKind.Absolute">absolute</see> <see cref="Uri">URI</see>.
		/// </summary>
		/// <returns>
		///		The request's absolute URI.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request has a <see cref="UriKind.Relative">relative</see> <see cref="Uri">URI</see>.
		/// </exception>
		public static bool HasAbsoluteUri(this IHttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException(nameof(httpRequest));

			return httpRequest.RequestUri.IsAbsoluteUri;
		}

		/// <summary>
		///		Ensure that the <see cref="IHttpRequest"/> has an <see cref="UriKind.Absolute">absolute</see> <see cref="Uri">URI</see>.
		/// </summary>
		/// <returns>
		///		The request's absolute URI.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request has a <see cref="UriKind.Relative">relative</see> <see cref="Uri">URI</see>.
		/// </exception>
		public static Uri EnsureAbsoluteUri(this IHttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException(nameof(httpRequest));

			Uri requestUri = httpRequest.RequestUri;
			if (requestUri.IsAbsoluteUri)
				return requestUri;

			throw new InvalidOperationException("The HTTP request does not have an absolute URI.");
		}

		/// <summary>
		///		Create a copy of the request with the specified base URI.
		/// </summary>
		/// <param name="request">
		///		The request.
		/// </param>
		/// <param name="baseUri">
		///		The request base URI (must be absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request already has an absolute URI.
		/// </exception>
		public static HttpRequest WithBaseUri(this HttpRequest request, Uri baseUri)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (baseUri == null)
				throw new ArgumentNullException(nameof(baseUri));

			if (!baseUri.IsAbsoluteUri)
				throw new ArgumentException("The supplied base URI is not an absolute URI.", nameof(baseUri));

			if (request.RequestUri.IsAbsoluteUri)
				throw new InvalidOperationException("The request already has an absolute URI.");

			return request.Clone(properties =>
			{
				properties.SetRequestUri(
					baseUri.AppendRelativeUri(request.RequestUri)
				);
			});
		}

		/// <summary>
		///		Create a copy of the request with the specified request URI.
		/// </summary>
		/// <param name="request">
		///		The request.
		/// </param>
		/// <param name="requestUri">
		///		The new request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithRequestUri(this HttpRequest request, Uri requestUri)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			return request.Clone(properties =>
			{
				properties.SetRequestUri(requestUri);
			});
		}

		/// <summary>
		///		Create a copy of the request with the specified request URI appended to its existing URI.
		/// </summary>
		/// <param name="request">
		///		The request.
		/// </param>
		/// <param name="relativeUri">
		///		The relative request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithRelativeRequestUri(this HttpRequest request, string relativeUri)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(relativeUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'relativeUri'.", nameof(relativeUri));

			return request.WithRelativeRequestUri(
				new Uri(relativeUri, UriKind.Relative)
			);
		}

		/// <summary>
		///		Create a copy of the request with the specified request URI appended to its existing URI.
		/// </summary>
		/// <param name="request">
		///		The request.
		/// </param>
		/// <param name="relativeUri">
		///		The relative request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithRelativeRequestUri(this HttpRequest request, Uri relativeUri)
		{
			if (relativeUri == null)
				throw new ArgumentNullException(nameof(relativeUri));

			if (relativeUri.IsAbsoluteUri)
				throw new ArgumentException("The specified URI is not a relative URI.", nameof(relativeUri));

			return request.Clone(properties =>
			{
				properties.SetRequestUri(
					request.RequestUri.AppendRelativeUri(relativeUri)
				);
			});
		}
	}
}
