package tasks;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;

import twinlib.Twin;

public class RegisterTask implements TwinTask {

	private Future<Boolean> future;
	private String matrix;

	public RegisterTask(String matrix) {
		this.future = Twin.register(matrix);
		this.matrix = matrix;
	}

	@Override
	public boolean isDone() {
		return this.future.isDone();
	}

	public List<String> get() throws InterruptedException, ExecutionException {
		Boolean result = future.get();
		if (result == null) {
			return Collections.singletonList("Internal error. Unable to register matrix");
		} else if (result) {
			return Collections.singletonList("Matrix " + matrix + " registered successfully");
		} else {
			return Collections.singletonList("Matrix " + matrix + " is already registered");
		}
	}

}
