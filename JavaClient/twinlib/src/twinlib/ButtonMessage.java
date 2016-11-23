package twinlib;

import twinlib.api.TwinButton;
import twinlib.api.TwinButtonMessage;
import twinlib.api.TwinMessageType;

public class ButtonMessage implements TwinButtonMessage {

	private final String user;
	private final TwinButton button;
	
	public ButtonMessage(String user, TwinButton button) {
		this.user = user;
		this.button = button;
	}

	@Override
	public String getUser() {
		return user;
	}

	@Override
	public TwinMessageType getMessageType() {
		return TwinMessageType.BUTTON;
	}

	@Override
	public TwinButton getClickedButton() {
		return this.button;
	}

	@Override
	public String toString() {
		return "ButtonMessage [user=" + user + ", button=" + button + ", getClass()=" + getClass() + "]";
	}

}
