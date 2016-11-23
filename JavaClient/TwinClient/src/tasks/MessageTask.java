package tasks;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Future;

import twinlib.ButtonMessage;
import twinlib.TextMessage;
import twinlib.Twin;
import twinlib.api.TwinMessage;
import twinlib.api.TwinMessageType;

public class MessageTask implements TwinTask {

	private Future<List<TwinMessage>> future;
	private String matrix;
	
	public MessageTask(String matrix) {
		this.future = Twin.readMessages(matrix);
		this.matrix = matrix;
	}

	@Override
	public boolean isDone() {
		return this.future.isDone();
	}

	@Override
	public List<String> get() throws Exception {
		List<TwinMessage> messages = this.future.get();
		List<String> toReturn = new ArrayList<>();
		for(TwinMessage msg : messages) {
			if(msg.getMessageType() == TwinMessageType.BUTTON) {
				ButtonMessage btnmsg = (ButtonMessage) msg;
				toReturn.add(matrix + "@" + btnmsg.getUser() + ": " + btnmsg.getClickedButton());
			} else {
				TextMessage txtmsg = (TextMessage) msg;
				toReturn.add(matrix + "@" + txtmsg.getUser() + ": " + txtmsg.getTextMessage());
			}
		}
		
		return toReturn;
	}
	
}
