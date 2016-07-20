namespace WLMHash {
    public class WNameHash {
        public static int Compute(string a) {
            uint v = 0, v2 = 0;
            foreach (char c in a) {
                v = (v << 4) + ((uint)c);
                v2 = (v2 << 4) + ((v >> 28) & 15);
            }
            return (int)(v + v2);
        }
    }
}
