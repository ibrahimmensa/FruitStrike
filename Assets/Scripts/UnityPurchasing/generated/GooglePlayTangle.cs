// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("j8z+7NVmmP4EvVl5ZZu8mbJUE/6bGBYZKZsYExubGBgZleNwNKbR2hfjRXjLhwhBGxB0frh0ixbYnj4FeexF472MjUlmBHz5N3SR1THYT2cpmxg7KRQfEDOfUZ/uFBgYGBwZGqNec5iq5mXZGFKtzZTCw1hsd/OCI/j0kW8EplVAXrhqec09arDSmxCQv3hvh2dwMZSXthpIs/eIor6ppQub3UJKI8pXmrFZSH7yNEo4QnUs4Xd1muqZyAOSkZblBaI8MrKlpvbHWfRPhWUCZ3WHHy1XreDIegAnHxuZxrHZ1O/TZILGHpMeQBsIuZOkWKb5nXM5aQIBaS0WfWJyhhv2BqDu45XtK8SXZvfep20r34DlWWe+Ar+oLHKErdT7SBsaGBkY");
        private static int[] order = new int[] { 11,1,13,9,11,13,13,8,9,11,13,13,12,13,14 };
        private static int key = 25;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
