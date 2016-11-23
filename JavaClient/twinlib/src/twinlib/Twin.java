package twinlib;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.ThreadFactory;

import org.json.JSONArray;

import twinlib.api.TwinButton;
import twinlib.api.TwinMessage;

public class Twin {

	private static final String ADDRESS = "http://webledmatrix.azurewebsites.net/clientApi";
	private static final ExecutorService pool = Executors.newSingleThreadScheduledExecutor(new ThreadFactory() {

		@Override
		public Thread newThread(Runnable r) {
			Thread t = Executors.defaultThreadFactory().newThread(r);
			t.setDaemon(true);
			return t;
		}
	});

	/**
	 * @return true if matrix got registered, false if matrix is already
	 *         registered and null otherwise (error).
	 */
	public static Future<Boolean> register(String matrixName) {
		if (matrixName == null) {
			throw new NullPointerException("matrixName");
		}
		return pool.submit(new Callable<Boolean>() {

			@Override
			public Boolean call() throws Exception {

				String result = readFromSite(
						new URL(ADDRESS + "/Register/" + matrixName)).toLowerCase()
								.replaceAll("\"", "");
				if ("refreshed".equals(result)) {
					return false;
				} else if ("registered".equals(result)) {
					return true;
				} else {
					return null;
				}
			}
		});
	}
	
	

	private static String readFromSite(URL url) throws IOException {
		HttpURLConnection conn = (HttpURLConnection) url.openConnection();
		conn.setRequestMethod("GET");
		conn.setRequestProperty("Accept", "application/json");
		if (conn.getResponseCode() != 200) {
			return null;
		}

		BufferedReader br = new BufferedReader(new InputStreamReader(conn.getInputStream()));
		Scanner sc = new Scanner(br);
		StringBuilder sb = new StringBuilder();
		while (sc.hasNext()) {
			sb.append(sc.nextLine());
		}
		conn.disconnect();
		br.close();
		sc.close();
		return sb.toString();
	}

	public static Future<List<TwinMessage>> readMessages(String matrixName) {
		if (matrixName == null) {
			throw new NullPointerException("matrixName");
		}
		return pool.submit(new Callable<List<TwinMessage>>() {

			@Override
			public List<TwinMessage> call() throws Exception {
				try {
					List<TwinMessage> operations = new ArrayList<>();
					URL url = new URL(ADDRESS + "/Commands/" + matrixName);
					String result = readFromSite(url);
					if(result==null) return operations;
					JSONArray obj = new JSONArray(result);

					for (Object a : obj) {
						String str = (String) a;
						String[] split = str.split(":");
						String user = split[0];
						String msg = split[1].toLowerCase();
						final TwinMessage message;
						if ("ok".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.OK);
						} else if ("send".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.SEND);
						} else if ("exit".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.EXIT);
						} else if ("up".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.UP);
						} else if ("down".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.DOWN);
						} else if ("right".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.RIGHT);
						} else if ("left".equals(msg)) {
							message = new ButtonMessage(user, TwinButton.LEFT);
						} else {
							message = new TextMessage(user, split[1]);
							System.out.println(message);
						}
						operations.add(message);
					}

					return operations;
				} catch (MalformedURLException e) {

					e.printStackTrace();

				} catch (IOException e) {

					e.printStackTrace();
				}
				return null;
			}
		});

	}

}
