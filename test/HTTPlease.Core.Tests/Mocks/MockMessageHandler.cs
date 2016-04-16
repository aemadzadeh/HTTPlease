﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Tests.Mocks
{
	/// <summary>
	///		Mock <see cref="DelegatingHandler"/> that calls an arbitrary delegate to receive and respond to a message.
	/// </summary>
	public sealed class MockMessageHandler
		: DelegatingHandler
	{
		/// <summary>
		///		The handler implementation.
		/// </summary>
		readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handlerImplementation;

		/// <summary>
		///		Create a new mock message handler.
		/// </summary>
		/// <param name="handlerImplementation">
		///		The handler implementation.
		/// </param>
		public MockMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handlerImplementation)
		{
			if (handlerImplementation == null)
				throw new ArgumentNullException(nameof(handlerImplementation));

			_handlerImplementation = request => Task<HttpResponseMessage>.Factory.StartNew(
				() => handlerImplementation(request)
			);
		}

		/// <summary>
		///		Create a new mock message handler.
		/// </summary>
		/// <param name="handlerImplementation">
		///		The handler implementation.
		/// </param>
		public MockMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerImplementation)
		{
			if (handlerImplementation == null)
				throw new ArgumentNullException(nameof(handlerImplementation));

			_handlerImplementation = handlerImplementation;
		}

		/// <summary>
		///		Asynchronously handle a request
		/// </summary>
		/// <param name="request">
		///		The request message.
		/// </param>
		/// <param name="cancellationToken">
		///		A cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task{TResult}"/> representing the asynchronous operation, whose result is the response message.
		/// </returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return await _handlerImplementation(request);
		}
	}
}
