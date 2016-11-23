package twinlib;

import twinlib.api.TwinMessageType;
import twinlib.api.TwinTextMessage;

public class TextMessage implements TwinTextMessage {

	private final String user, message;

	public TextMessage(String user, String message) {
		this.user = user;
		this.message = message;
	}

	@Override
	public String getUser() {
		return this.user;
	}

	@Override
	public TwinMessageType getMessageType() {
		return TwinMessageType.TEXT;
	}

	@Override
	public String getTextMessage() {
		return this.message;
	}

	@Override
	public String toString() {
		return "TextMessage [user=" + this.user + ", message=" + this.message + ", getClass()=" + getClass() + "]";
	}

}
