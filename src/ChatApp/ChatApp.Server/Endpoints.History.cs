﻿
using ChatApp.Server.Models;
using ChatApp.Server.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace ChatApp.Server;

public static partial class Endpoints
{
    public static WebApplication MapHistoryEndpoints(this WebApplication app)
    {
        app.MapGet("/history/ensure", GetEnsureHistoryAsync);

        // Not implemented
        app.MapPost("/history/generate", GenerateHistory);
        app.MapPost("/history/update", UpdateHistory);
        app.MapPost("/history/message_feedback", MessageFeedback);
        app.MapDelete("/history/delete", DeleteHistory);
        app.MapGet("/history/list", ListHistory);
        app.MapPost("/history/read", ReadHistory);
        app.MapPost("/history/rename", RenameHistory);
        app.MapDelete("/history/delete_all", DeleteAllHistory);
        app.MapPost("/history/clear", ClearHistoryAsync);

        return app;
    }

    private static async Task<IResult> GetEnsureHistoryAsync(HttpContext httpContext, [FromServices] CosmosConversationService conversationService)
    {
        // todo: refactor the UI so that this is can be refactored to make any amount of sense...
        var (cosmosIsConfigured, exception) = await conversationService.EnsureAsync();

        return cosmosIsConfigured
            ? Results.Ok(JsonSerializer.Deserialize<object>(@"{ ""converation"": ""CosmosDB is configured and working""}"))
            : Results.NotFound(JsonSerializer.Deserialize<object>(@"{ ""error"": ""CosmosDB is not configured""}"));
    }

    #region NotImplemented
    private static async Task<IActionResult> ClearHistoryAsync(HttpContext context, [FromServices] CosmosConversationService conversationService)
    {
        // get the user id from the request headers
        var user = GetUser(context);

        if (user == null)
            return new UnauthorizedResult();
        //conversation_id = request_json.get("conversation_id", None)

        // check request for conversation_id
        var conversation = await context.GetFromRequestJsonAsync<Conversation>();

        if (string.IsNullOrWhiteSpace(conversation?.Id))
            return new BadRequestObjectResult("conversation_id is required");

        // todo: do conversations and messages need to be deleted separately
        // or can the conversation delete in the service encompass the messages
        // delete the conversation messages from cosmos
        var deleted = await conversationService.DeleteConversationAsync(user.UserPrincipalId, conversation.Id);

        return deleted
            ? new OkObjectResult(new { message = "Successfully deleted messages in conversation", conversation_id = conversation.Id })
            : new NotFoundResult();
    }

    private static async Task DeleteAllHistory(HttpContext context)
    {
        //## get the user id from the request headers
        //authenticated_user = get_authenticated_user_details(request_headers = request.headers)
        //user_id = authenticated_user["user_principal_id"]

        //# get conversations for user
        //try:
        //    ## make sure cosmos is configured
        //    cosmos_conversation_client = init_cosmosdb_client()
        //    if not cosmos_conversation_client:
        //        raise Exception("CosmosDB is not configured or not working")

        //    conversations = await cosmos_conversation_client.get_conversations(
        //        user_id, offset = 0, limit = None
        //    )
        //    if not conversations:
        //        return jsonify({ "error": f"No conversations for {user_id} were found"}), 404

        //    # delete each conversation
        //    for conversation in conversations:
        //        ## delete the conversation messages from cosmos first
        //        deleted_messages = await cosmos_conversation_client.delete_messages(
        //            conversation["id"], user_id
        //        )

        //        ## Now delete the conversation
        //        deleted_conversation = await cosmos_conversation_client.delete_conversation(
        //            user_id, conversation["id"]
        //        )
        //    await cosmos_conversation_client.cosmosdb_client.close()
        //    return (
        //        jsonify(
        //            {
        //        "message": f"Successfully deleted conversation and messages for user {user_id}"
        //            }
        //        ),
        //        200,
        //    )

        //except Exception as e:
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task RenameHistory(HttpContext context)
    {
        //authenticated_user = get_authenticated_user_details(request_headers = request.headers)
        //user_id = authenticated_user["user_principal_id"]

        //## check request for conversation_id
        //request_json = await request.get_json()
        //conversation_id = request_json.get("conversation_id", None)

        //if not conversation_id:
        //        return jsonify({ "error": "conversation_id is required"}), 400

        //## make sure cosmos is configured
        //cosmos_conversation_client = init_cosmosdb_client()
        //if not cosmos_conversation_client:
        //        raise Exception("CosmosDB is not configured or not working")

        //## get the conversation from cosmos
        //conversation = await cosmos_conversation_client.get_conversation(
        //    user_id, conversation_id
        //)
        //if not conversation:
        //        return (
        //            jsonify(
        //            {
        //        "error": f"Conversation {conversation_id} was not found. It either does not exist or the logged in user does not have access to it."
        //            }
        //        ),
        //        404,
        //    )

        //## update the title
        //title = request_json.get("title", None)
        //if not title:
        //        return jsonify({ "error": "title is required"}), 400
        //conversation["title"] = title
        //updated_conversation = await cosmos_conversation_client.upsert_conversation(
        //    conversation
        //)

        //await cosmos_conversation_client.cosmosdb_client.close()
        //return jsonify(updated_conversation), 200
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task ReadHistory(HttpContext context)
    {
        //authenticated_user = get_authenticated_user_details(request_headers = request.headers)
        //user_id = authenticated_user["user_principal_id"]

        //## check request for conversation_id
        //request_json = await request.get_json()
        //conversation_id = request_json.get("conversation_id", None)

        //if not conversation_id:
        //        return jsonify({ "error": "conversation_id is required"}), 400

        //## make sure cosmos is configured
        //cosmos_conversation_client = init_cosmosdb_client()
        //if not cosmos_conversation_client:
        //        raise Exception("CosmosDB is not configured or not working")

        //## get the conversation object and the related messages from cosmos
        //conversation = await cosmos_conversation_client.get_conversation(
        //    user_id, conversation_id
        //)
        //## return the conversation id and the messages in the bot frontend format
        //if not conversation:
        //        return (
        //            jsonify(
        //            {
        //        "error": f"Conversation {conversation_id} was not found. It either does not exist or the logged in user does not have access to it."
        //            }
        //        ),
        //        404,
        //    )

        //# get the messages for the conversation from cosmos
        //conversation_messages = await cosmos_conversation_client.get_messages(
        //    user_id, conversation_id
        //)

        //## format the messages in the bot frontend format
        //messages = [
        //    {
        //        "id": msg["id"],
        //        "role": msg["role"],
        //        "content": msg["content"],
        //        "createdAt": msg["createdAt"],
        //        "feedback": msg.get("feedback"),
        //    }
        //    for msg in conversation_messages
        //]

        //await cosmos_conversation_client.cosmosdb_client.close()
        //return jsonify({ "conversation_id": conversation_id, "messages": messages}), 200
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task ListHistory(HttpContext context)
    {
        //offset = request.args.get("offset", 0)
        //authenticated_user = get_authenticated_user_details(request_headers = request.headers)
        //user_id = authenticated_user["user_principal_id"]

        //## make sure cosmos is configured
        //cosmos_conversation_client = init_cosmosdb_client()
        //if not cosmos_conversation_client:
        //        raise Exception("CosmosDB is not configured or not working")

        //## get the conversations from cosmos
        //conversations = await cosmos_conversation_client.get_conversations(
        //    user_id, offset = offset, limit = 25
        //)
        //await cosmos_conversation_client.cosmosdb_client.close()
        //if not isinstance(conversations, list):
        //    return jsonify({ "error": f"No conversations for {user_id} were found"}), 404

        //## return the conversation ids

        //return jsonify(conversations), 200
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task<IActionResult> DeleteHistory(HttpContext context, [FromServices] CosmosConversationService conversationService)
    {
        var user = GetUser(context);

        if (user == null)
            return new UnauthorizedResult();

        var converation = await context.GetFromRequestJsonAsync<Conversation>();

        var deletedConvo = await conversationService.DeleteConversationAsync(user.UserPrincipalId, converation.Id);

        var response = new
        {
            message = "Successfully deleted conversation and messages",
            conversation_id = converation.Id
        };

        return new OkObjectResult(response);
    }

    private static async Task<IActionResult> MessageFeedback(HttpContext context, [FromServices] CosmosConversationService conversationService)
    {
        var user = GetUser(context);

        if (user == null)
            return new UnauthorizedResult();

        // todo: identify what the incoming request should look like
        var message = await context.GetFromRequestJsonAsync<HistoryMessage>();

        var updatedMessage = await conversationService.UpdateMessageFeedbackAsync(user.UserPrincipalId, message.Id, message.Feedback);

        return updatedMessage != null
            ? new OkObjectResult(updatedMessage)
            : new NotFoundResult();
    }

    private static async Task UpdateHistory(HttpContext context)
    {
        //authenticated_user = get_authenticated_user_details(request_headers = request.headers)
        //user_id = authenticated_user["user_principal_id"]

        //## check request for conversation_id
        //request_json = await request.get_json()
        //conversation_id = request_json.get("conversation_id", None)

        //try:
        //    # make sure cosmos is configured
        //    cosmos_conversation_client = init_cosmosdb_client()
        //    if not cosmos_conversation_client:
        //        raise Exception("CosmosDB is not configured or not working")

        //    # check for the conversation_id, if the conversation is not set, we will create a new one
        //    if not conversation_id:
        //        raise Exception("No conversation_id found")

        //    ## Format the incoming message object in the "chat/completions" messages format
        //    ## then write it to the conversation history in cosmos
        //    messages = request_json["messages"]
        //    if len(messages) > 0 and messages[-1]["role"] == "assistant":
        //        if len(messages) > 1 and messages[-2].get("role", None) == "tool":
        //            # write the tool message first
        //            await cosmos_conversation_client.create_message(
        //                uuid = str(uuid.uuid4()),
        //                conversation_id = conversation_id,
        //                user_id = user_id,
        //                input_message = messages[-2],
        //            )
        //        # write the assistant message
        //        await cosmos_conversation_client.create_message(
        //            uuid = messages[-1]["id"],
        //            conversation_id = conversation_id,
        //            user_id = user_id,
        //            input_message = messages[-1],
        //        )
        //    else:
        //        raise Exception("No bot messages found")

        //    # Submit request to Chat Completions for response
        //    await cosmos_conversation_client.cosmosdb_client.close()
        //    response = { "success": True}
        //    return jsonify(response), 200

        //except Exception as e:
        //    logging.exception("Exception in /history/update")
        //    return jsonify({ "error": str(e)}), 500
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task<T?> GetFromRequestJsonAsync<T>(this HttpContext context)
    {
        using var streamReader = new StreamReader(context.Request.Body);

        var json = await streamReader.ReadToEndAsync();

        var obj = JsonSerializer.Deserialize<T>(json);

        return obj;
    }

    private static async Task<IActionResult> GenerateHistory(HttpContext context, [FromServices] CosmosConversationService conversationService)
    {
        var authenticated_user = GetUser(context);
        var user_id = authenticated_user.UserPrincipalId;

        // check request for conversation_id

        // mj: what should request body look like?
        var conversation = await context.GetFromRequestJsonAsync<Conversation>();

        // todo: better handling of bad reqs
        if (conversation == null)
            return new BadRequestResult();

        var history_metadata = new Dictionary<string, object>();

        if (string.IsNullOrWhiteSpace(conversation.Id))
        {
            var title = await GenerateTitleAsync(conversation.Messages);
            var newConversation = await conversationService.CreateConversationAsync(user_id, title);
            history_metadata["title"] = conversation.Title;
            history_metadata["date"] = conversation.CreatedAt;
        }

        // Format the incoming message object in the "chat/completions" messages format
        // then write it to the conversation history in cosmos

        //if (conversation.Messages.Count > 0 && conversation.Messages[0].Role == "User") // move role format to enum?
        //{
            // todo: sort out the 
            //var message = new Message(conversation_id, user_id, (Dictionary<string, object>)messages[0]);


        //}

        // todo: build out history and send to conversations endpoint

        //        if len(messages) > 0 and messages[-1]["role"] == "user":
        //            createdMessageValue = await cosmos_conversation_client.create_message(
        //                uuid = str(uuid.uuid4()),
        //                conversation_id = conversation_id,
        //                user_id = user_id,
        //                input_message = messages[-1],
        //            )
        //            if createdMessageValue == "Conversation not found":
        //                raise Exception(
        //                    "Conversation not found for the given conversation ID: "
        //                    + conversation_id
        //                    + "."
        //                )
        //        else:
        //            raise Exception("No user message found")

        //        await cosmos_conversation_client.cosmosdb_client.close()

        //        # Submit request to Chat Completions for response
        //        request_body = await request.get_json()
        //        history_metadata["conversation_id"] = conversation_id
        //        request_body["history_metadata"] = history_metadata
        //        return await conversation_internal(request_body, request.headers)

        //    except Exception as e:
        //        logging.exception("Exception in /history/generate")
        //        return jsonify({ "error": str(e)}), 500

        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static async Task<string> GenerateTitleAsync(object messages)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    private static EasyAuthUser? GetUser(HttpContext context)
    {
        // return a default user if we're in development mode otherwise return null
        if (!context.Request.Headers.TryGetValue("X-Ms-Client-Principal-Id", out var principalId))
            return !string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase)
                ? new() // todo: should we also use a test static json of Easy Auth headers to be injected into HttpContext in development mode?
                : null;

        return new EasyAuthUser
        {
            UserPrincipalId = principalId.FirstOrDefault() ?? string.Empty,
            Username = context.Request.Headers["X-Ms-Client-Principal-Name"].FirstOrDefault() ?? string.Empty,
            AuthProvider = context.Request.Headers["X-Ms-Client-Principal-Idp"].FirstOrDefault() ?? string.Empty,
            AuthToken = context.Request.Headers["X-Ms-Token-Aad-Id-Token"].FirstOrDefault() ?? string.Empty,
            ClientPrincipalB64 = context.Request.Headers["X-Ms-Client-Principal"].FirstOrDefault() ?? string.Empty,
            AadIdToken = context.Request.Headers["X-Ms-Token-Aad-Id-Token"].FirstOrDefault() ?? string.Empty
        };
    }
    #endregion
}
